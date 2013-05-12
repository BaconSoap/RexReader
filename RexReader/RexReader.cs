using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Varnerin.RexTools {
    /// <summary>
    /// Reads a compressed .xp stream and provides methods to read the data.
    /// Every operation resets the underlying stream, so feel free to call every method as many times as you want.
    /// It also caches results in case you are too lazy to set up caching on your end.
    /// </summary>
    public class RexReader {
        private Stream Deflated { get; set; }
        private BinaryReader Reader { get; set; }
        private int? _layers;

        /// <summary>
        /// Construct a RexReader from a compressed stream (of the .xp format)
        /// </summary>
        /// <param name="inputStream">The compressed stream of the .xp file</param>
        public RexReader(Stream inputStream) {
            SetupFromStream(inputStream);
        }

        /// <summary>
        /// Construct a RexReader from a compressed stream (of the .xp format)
        /// </summary>
        /// <param name="inputStream"></param>
        private void SetupFromStream(Stream inputStream) {
            Deflated = new MemoryStream();
            inputStream.Position = 0;
            using (var deflate = new GZipStream(inputStream, CompressionMode.Decompress)) {
                deflate.CopyTo(Deflated);
            }

            Reader = new BinaryReader(Deflated);
            Deflated.Position = 0;
        }

        /// <summary>
        /// Construct a RexReader from an .xp file. Throws standard errors if the file doesn't exist.
        /// </summary>
        /// <param name="filePath">Path to the .xp file</param>
        public RexReader(string filePath) {
            using (var memoryStream = new MemoryStream()) {
                using (var filestream = new FileStream(filePath, FileMode.Open)) {
                    filestream.CopyTo(memoryStream);
                }
                SetupFromStream(memoryStream);
            }
        }

        /// <summary>
        /// Retrieve the number of layers in the image
        /// </summary>
        /// <returns>Number of layers in image</returns>
        public int GetLayerCount() {
            if (_layers.HasValue) {
                return _layers.Value;
            }


            int layerCount = Reader.ReadInt32();
            Deflated.Position = 0;
            _layers = layerCount;
            return layerCount;
        }

        /// <summary>
        /// Gets the width of the layer specified. Throws 
        /// </summary>
        /// <param name="layer">The 0-based layer number</param>
        /// <returns>The width in cells of the specified layer</returns>
        public int GetLayerWidth(int layer) {
            if (layer < 0 || layer >= GetLayerCount())
                throw new ArgumentOutOfRangeException("layer");
            var offset = (32 + layer * 64) / 8;

            Deflated.Seek(offset, SeekOrigin.Begin);
            var width = Reader.ReadInt32();
            Deflated.Seek(0, SeekOrigin.Begin);
            return width;
        }

        /// <summary>
        /// Gets the height of the layer specified. Throws 
        /// </summary>
        /// <param name="layer">The 0-based layer number</param>
        /// <returns>The height in cells of the specified layer</returns>
        public int GetLayerHeight(int layer) {
            if (layer < 0 || layer >= GetLayerCount())
                throw new ArgumentOutOfRangeException("layer");
            var offset = (32 + 32 + layer * 64) / 8;

            Deflated.Seek(offset, SeekOrigin.Begin);
            var height = Reader.ReadInt32();
            Deflated.Seek(0, SeekOrigin.Begin);
            return height;
        }

        /// <summary>
        /// Reads the characters of the specified layer into a string and returns it. No newlines/colors are included.
        /// Returned string goes down, then right (down each column and wraps up to the top on a new column) ie column-major.
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public string ReadLayerAsString(int layer) {
            if (layer < 0 || layer >= GetLayerCount())
                throw new ArgumentOutOfRangeException("layer");

            var count = GetLayerWidth(layer) * GetLayerHeight(layer);
            var builder = new StringBuilder(count);

            //Find the starting offset of the layer
            var offset = GetFirstTileOffset();
            Deflated.Seek(offset, SeekOrigin.Begin);
            const int charColorSize = 6;
            for (var i = 0; i < count; i++) {
                builder.Append((char)Reader.ReadInt32());
                Deflated.Seek(charColorSize, SeekOrigin.Current);
            }
            Deflated.Seek(0, SeekOrigin.Begin);
            return builder.ToString();
        }

        private int GetFirstTileOffset() {
            return (32 + GetLayerCount() * 64) / 8;
        }

        /// <summary>
        /// Retrieves the entire map, including all its layers. Row-major order for tiles (y, then x).
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Note that (255,0,255) looks to be the 'transparent' code. But only for backgrounds?
        /// If you see magenta where you thought was black, that's why.
        /// </remarks>
        public TileMap GetMap() {
            int layers;
            int width;
            int height;

            //Get out early if the data is corrupt/malformed
            try {
                layers = GetLayerCount();
                width = GetLayerWidth(0);
                height = GetLayerHeight(0);
            } catch (Exception e) {
                Reader.Dispose();
                throw new InvalidDataException("Bad .xp data", e);
            }
            var map = new TileMap(width, height, layers);

            //Move to the first tile
            Deflated.Seek(GetFirstTileOffset(), SeekOrigin.Begin);

            //Read every tile in column-major order, and place it in the map in row-major order
            for (var layer = 0; layer < layers; layer++) {
                for (var col = 0; col < height; col++) {
                    for (var row = 0; row < width; row++) {
                        map.Layers[layer].Tiles[row, col] = new Tile {
                            CharacterCode = (byte)Reader.ReadInt32(),
                            ForegroundRed = Reader.ReadByte(),
                            ForegroundGreen = Reader.ReadByte(),
                            ForegroundBlue = Reader.ReadByte(),
                            BackgroundRed = Reader.ReadByte(),
                            BackgroundGreen = Reader.ReadByte(),
                            BackgroundBlue = Reader.ReadByte()
                        };
                    }
                }
            }
            Deflated.Seek(0, SeekOrigin.Begin);
            return map;
        }

    }
}
