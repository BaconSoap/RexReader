using System;
using System.IO;
using NUnit.Framework;
using RexReader;
namespace RexReaderTests {
    [TestFixture]
    public class RexReaderTest {

        private string SingleLayer = "data" + Path.DirectorySeparatorChar + "singleLayer.xp";
        private const string SingleLayerString = "uA                                                                                                 e";
        private const int SingleLayerWidth = 10;
        private const int SingleLayerHeight = 10;
        private const int SingleLayerLayerCount = 1;
        private Stream SingleLayerStream { get; set; }
        public RexReader.RexReader SingleReader { get { return new RexReader.RexReader(SingleLayerStream); } }
        public const byte Space = (byte)' ';

        [SetUp]
        public void SetupStream() {
            var memoryStream = new MemoryStream();
            using (var filestream = new FileStream(SingleLayer, FileMode.Open)) {
                filestream.CopyTo(memoryStream);
            }
            SingleLayerStream = memoryStream;
        }

        public TileMap GetSingleLayerTestMap() {
            var map = new TileMap(SingleLayerWidth, SingleLayerHeight, SingleLayerLayerCount);
            for (var row = 0; row < map.Height; row++) {
                for (var col = 0; col < map.Width; col++) {
                    var tile = new Tile();
                    tile.CharacterCode = Space;
                    tile.BackgroundBlue = 255;
                    tile.BackgroundGreen = 0;
                    tile.BackgroundRed = 255;
                    tile.ForegroundBlue = 0;
                    tile.ForegroundGreen = 0;
                    tile.ForegroundRed = 0;

                    map.Layers[0].Tiles[row, col] = tile;
                }
            }

            //First tile
            map.Layers[0].Tiles[0, 0].CharacterCode = (byte)'u';
            map.Layers[0].Tiles[0, 0].BackgroundRed = 255;
            map.Layers[0].Tiles[0, 0].BackgroundBlue = 0;
            map.Layers[0].Tiles[0, 0].ForegroundGreen = 255;

            //First tile in second row
            map.Layers[0].Tiles[1, 0].CharacterCode = (byte)'A';
            map.Layers[0].Tiles[1, 0].BackgroundBlue = 255;
            map.Layers[0].Tiles[1, 0].BackgroundRed = 0;
            map.Layers[0].Tiles[1, 0].ForegroundRed = 255;

            //Last tile in last row
            map.Layers[0].Tiles[9, 9].CharacterCode = (byte)'e';
            map.Layers[0].Tiles[9, 9].BackgroundBlue = 255;
            map.Layers[0].Tiles[9, 9].BackgroundRed = 0;
            map.Layers[0].Tiles[9, 9].ForegroundBlue = 255;

            return map;
        }

        [Test]
        public void New_RexReader_Saves_Stream() {
            Assert.That(SingleReader.BaseStream, Is.EqualTo(SingleLayerStream));
        }

        [Test]
        public void GetLayerCount_With_Single_Layer_Returns_1() {
            Assert.That(SingleReader.GetLayerCount(), Is.EqualTo(1));
        }

        [Test]
        public void GetLayerWidth_Throws_ArgumentOutOfRangeException_For_Negative_Layer() {
            Assert.That(() => SingleReader.GetLayerWidth(-1), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetLayerWidth_Throws_ArgumentOutOfRangeException_For_Layer_Greater_Than_Layer_Count() {
            Assert.That(() => SingleReader.GetLayerWidth(4), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetLayerWidth_Works_For_Single_Layer() {
            Assert.That(SingleReader.GetLayerWidth(0), Is.EqualTo(SingleLayerWidth));
        }

        [Test]
        public void GetLayerHeight_Throws_ArgumentOutOfRangeException_For_Negative_Layer() {
            Assert.That(() => SingleReader.GetLayerHeight(-1), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetLayerHeight_Throws_ArgumentOutOfRangeException_For_Layer_Greater_Than_Layer_Count() {
            Assert.That(() => SingleReader.GetLayerHeight(4), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetLayerHeight_Works_For_Single_Layer() {
            Assert.That(SingleReader.GetLayerHeight(0), Is.EqualTo(SingleLayerHeight));
        }

        [Test]
        public void ReadLayerAsString_Throws_ArgumentOutOfRangeException_For_Negative_Layer() {
            Assert.That(() => SingleReader.ReadLayerAsString(-1), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ReadLayerAsString_Throws_ArgumentOutOfRangeException_For_Layer_Greater_Than_Layer_Count() {
            Assert.That(() => SingleReader.ReadLayerAsString(1), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ReadLayerAsString_Works_For_Single_Layer() {
            Assert.That(SingleReader.ReadLayerAsString(0), Is.EqualTo(SingleLayerString));
        }

        [Test]
        public void GetMap_Throws_InvalidDataException_With_Bad_Data() {
            var badStream = new MemoryStream();
            var writer = new StreamWriter(badStream);
            writer.Flush();
            writer.Write("oh noes");

            var reader = new RexReader.RexReader(badStream);
            Assert.That(() => reader.GetMap(), Throws.Exception.TypeOf<InvalidDataException>());
        }

        [Test]
        public void GetMap_Stores_LayerCount() {
            Assert.That(SingleReader.GetMap().LayerCount, Is.EqualTo(SingleLayerLayerCount));
        }

        [Test]
        public void GetMap_Stores_Width() {
            Assert.That(SingleReader.GetMap().Width, Is.EqualTo(SingleLayerWidth));
        }

        [Test]
        public void GetMap_Stores_Height() {
            Assert.That(SingleReader.GetMap().Width, Is.EqualTo(SingleLayerHeight));
        }

        [Test]
        public void GetMap_Sets_Character_For_Single_Layer() {
            var expected = GetSingleLayerTestMap();
            var actual = SingleReader.GetMap();
            for (var row = 0; row < expected.Height; row++) {
                for (var col = 0; col < expected.Width; col++) {
                    Assert.That(expected.Layers[0].Tiles[row, col].CharacterCode, Is.EqualTo(actual.Layers[0].Tiles[row, col].CharacterCode));
                }
            }
        }

        [Test]
        public void GetMap_Sets_ForegroundRed_For_Single_Layer() {
            var expected = GetSingleLayerTestMap();
            var actual = SingleReader.GetMap();
            for (var row = 0; row < expected.Height; row++) {
                for (var col = 0; col < expected.Width; col++) {
                    Assert.That(expected.Layers[0].Tiles[row, col].ForegroundRed, Is.EqualTo(actual.Layers[0].Tiles[row, col].ForegroundRed));
                }
            }
        }

        [Test]
        public void GetMap_Sets_ForegroundGreen_For_Single_Layer() {
            var expected = GetSingleLayerTestMap();
            var actual = SingleReader.GetMap();
            for (var row = 0; row < expected.Height; row++) {
                for (var col = 0; col < expected.Width; col++) {
                    Assert.That(expected.Layers[0].Tiles[row, col].ForegroundGreen, Is.EqualTo(actual.Layers[0].Tiles[row, col].ForegroundGreen));
                }
            }
        }

        [Test]
        public void GetMap_Sets_ForegroundBlue_For_Single_Layer() {
            var expected = GetSingleLayerTestMap();
            var actual = SingleReader.GetMap();
            for (var row = 0; row < expected.Height; row++) {
                for (var col = 0; col < expected.Width; col++) {
                    Assert.That(expected.Layers[0].Tiles[row, col].ForegroundBlue, Is.EqualTo(actual.Layers[0].Tiles[row, col].ForegroundBlue));
                }
            }
        }

        [Test]
        public void GetMap_Sets_BackgroundRed_For_Single_Layer() {
            var expected = GetSingleLayerTestMap();
            var actual = SingleReader.GetMap();
            for (var row = 0; row < expected.Height; row++) {
                for (var col = 0; col < expected.Width; col++) {
                    Assert.That(actual.Layers[0].Tiles[row, col].BackgroundRed, Is.EqualTo(expected.Layers[0].Tiles[row, col].BackgroundRed), "row " + row + " col " + col);
                }
            }
        }

        [Test]
        public void GetMap_Sets_BackgroundGreen_For_Single_Layer() {
            var expected = GetSingleLayerTestMap();
            var actual = SingleReader.GetMap();
            for (var row = 0; row < expected.Height; row++) {
                for (var col = 0; col < expected.Width; col++) {
                    Assert.That(actual.Layers[0].Tiles[row, col].BackgroundGreen, Is.EqualTo(expected.Layers[0].Tiles[row, col].BackgroundGreen));
                }
            }
        }

        [Test]
        public void GetMap_Sets_BackgroundBlue_For_Single_Layer() {
            var expected = GetSingleLayerTestMap();
            var actual = SingleReader.GetMap();
            for (var row = 0; row < expected.Height; row++) {
                for (var col = 0; col < expected.Width; col++) {
                    Assert.That(actual.Layers[0].Tiles[row, col].BackgroundBlue, Is.EqualTo(expected.Layers[0].Tiles[row, col].BackgroundBlue), "row " + row + " col " + col);
                }
            }
        }
    }
}
