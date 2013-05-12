using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace RexReader {
    /// <summary>
    /// Reads a compressed .xp stream and provides methods to read the data
    /// </summary>
    public class RexReader {
        public Stream BaseStream { get; private set; }
        private Stream Deflated { get; set; }
        private BinaryReader Reader { get; set; }
        private int? _layers;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputStream"></param>
        public RexReader(Stream inputStream) {
            Deflated = new MemoryStream();
            inputStream.Position = 0;
            BaseStream = inputStream;
            using (var deflate = new GZipStream(BaseStream, CompressionMode.Decompress)) {
                deflate.CopyTo(Deflated);
            }

            Reader = new BinaryReader(Deflated);
            Deflated.Position = 0;
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
        /// Returned string goes down, then right (down each column and wraps up to the top on a new column).
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public string ReadLayerAsString(int layer) {
            if (layer < 0 || layer >= GetLayerCount())
                throw new ArgumentOutOfRangeException("layer");

            var count = GetLayerWidth(layer) * GetLayerHeight(layer);
            var builder = new StringBuilder(count);
            var offset = (32 + GetLayerCount() * 64) / 8;
            Deflated.Seek(offset, SeekOrigin.Begin);
            const int charColorSize = 6;
            for (var i = 0; i < count; i++) {
                builder.Append((char)Reader.ReadInt32());
                Deflated.Seek(charColorSize, SeekOrigin.Current);
            }
            Deflated.Seek(0, SeekOrigin.Begin);
            return builder.ToString();
        }

    }
}
