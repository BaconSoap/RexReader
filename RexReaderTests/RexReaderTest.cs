using System;
using System.IO;
using NUnit.Framework;
using RexTools;

namespace RexReaderTests {
    [TestFixture]
    public class RexReaderTest {

        public readonly string OldSingleLayer = "data" + Path.DirectorySeparatorChar + "singleLayer_unversioned.xp";
        public readonly string SingleLayer = "data" + Path.DirectorySeparatorChar + "singleLayer.xp";
        private const string SingleLayerString = "uA                                                                                                 e";
        private const int SingleLayerWidth = 10;
        private const int SingleLayerHeight = 10;
        private const int SingleLayerLayerCount = 1;
        private Stream OldSingleLayerStream { get; set; }
        private Stream SingleLayerStream { get; set; }
        public RexReader GetSingleReader(bool oldFormat)
        {
            var stream = oldFormat ? OldSingleLayerStream : SingleLayerStream;
            if (new Random().Next(2) == 0) {
                return new RexReader(stream);
            }

            return new RexReader(oldFormat? OldSingleLayer: SingleLayer);
        }
        


        public const byte Space = (byte)' ';

        [SetUp]
        public void SetupStream() {
            var memoryStream = new MemoryStream();
            using (var filestream = new FileStream(SingleLayer, FileMode.Open))
            {
                filestream.CopyTo(memoryStream);
            }
            SingleLayerStream = memoryStream;

            var memoryStreamOld = new MemoryStream();
            using (var filestream = new FileStream(OldSingleLayer, FileMode.Open))
            {
                filestream.CopyTo(memoryStreamOld);
            }
            OldSingleLayerStream = memoryStreamOld;
        }

        public TileMap GetSingleLayerTestMap() {
            var map = new TileMap(SingleLayerWidth, SingleLayerHeight, SingleLayerLayerCount);
            for (var row = 0; row < map.Height; row++) {
                for (var col = 0; col < map.Width; col++) {
                    var tile = new Tile {
                        CharacterCode = Space,
                        BackgroundBlue = 255,
                        BackgroundGreen = 0,
                        BackgroundRed = 255,
                        ForegroundBlue = 0,
                        ForegroundGreen = 0,
                        ForegroundRed = 0
                    };

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
        [TestCase(true)]
        [TestCase(false)]
        public void GetLayerCount_With_Single_Layer_Returns_1(bool oldFormat)
        {
            Assert.That(GetSingleReader(oldFormat).GetLayerCount(), Is.EqualTo(1));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetLayerWidth_Throws_ArgumentOutOfRangeException_For_Negative_Layer(bool oldFormat)
        {
            Assert.That(() => GetSingleReader(oldFormat).GetLayerWidth(-1), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetLayerWidth_Throws_ArgumentOutOfRangeException_For_Layer_Greater_Than_Layer_Count(bool oldFormat)
        {
            Assert.That(() => GetSingleReader(oldFormat).GetLayerWidth(4), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetLayerWidth_Works_For_Single_Layer(bool oldFormat)
        {
            Assert.That(GetSingleReader(oldFormat).GetLayerWidth(0), Is.EqualTo(SingleLayerWidth));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetLayerHeight_Throws_ArgumentOutOfRangeException_For_Negative_Layer(bool oldFormat)
        {
            Assert.That(() => GetSingleReader(oldFormat).GetLayerHeight(-1), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetLayerHeight_Throws_ArgumentOutOfRangeException_For_Layer_Greater_Than_Layer_Count(bool oldFormat)
        {
            Assert.That(() => GetSingleReader(oldFormat).GetLayerHeight(4), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetLayerHeight_Works_For_Single_Layer(bool oldFormat)
        {
            Assert.That(GetSingleReader(oldFormat).GetLayerHeight(0), Is.EqualTo(SingleLayerHeight));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ReadLayerAsString_Throws_ArgumentOutOfRangeException_For_Negative_Layer(bool oldFormat)
        {
            Assert.That(() => GetSingleReader(oldFormat).ReadLayerAsString(-1), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ReadLayerAsString_Throws_ArgumentOutOfRangeException_For_Layer_Greater_Than_Layer_Count(bool oldFormat)
        {
            Assert.That(() => GetSingleReader(oldFormat).ReadLayerAsString(1), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ReadLayerAsString_Works_For_Single_Layer(bool oldFormat)
        {
            Assert.That(GetSingleReader(oldFormat).ReadLayerAsString(0), Is.EqualTo(SingleLayerString));
        }

        [Test]
        public void Constructor_Throws_InvalidDataException_With_Bad_Data() {
            var badStream = new MemoryStream();
            var writer = new StreamWriter(badStream);
            writer.Write("oh noes, what terrible data!");
            writer.Flush();

            Assert.That(() => new RexReader(badStream), Throws.Exception.TypeOf<InvalidDataException>());
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetMap_Stores_LayerCount(bool oldFormat) {
            Assert.That(GetSingleReader(oldFormat).GetMap().LayerCount, Is.EqualTo(SingleLayerLayerCount));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetMap_Stores_Width(bool oldFormat)
        {
            Assert.That(GetSingleReader(oldFormat).GetMap().Width, Is.EqualTo(SingleLayerWidth));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetMap_Stores_Height(bool oldFormat)
        {
            Assert.That(GetSingleReader(oldFormat).GetMap().Width, Is.EqualTo(SingleLayerHeight));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetMap_Sets_Character_For_Single_Layer(bool oldFormat)
        {
            var expected = GetSingleLayerTestMap();
            var actual = GetSingleReader(oldFormat).GetMap();
            for (var row = 0; row < expected.Height; row++) {
                for (var col = 0; col < expected.Width; col++) {
                    Assert.That(expected.Layers[0].Tiles[row, col].CharacterCode, Is.EqualTo(actual.Layers[0].Tiles[row, col].CharacterCode));
                }
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetMap_Sets_ForegroundRed_For_Single_Layer(bool oldFormat)
        {
            var expected = GetSingleLayerTestMap();
            var actual = GetSingleReader(oldFormat).GetMap();
            for (var row = 0; row < expected.Height; row++) {
                for (var col = 0; col < expected.Width; col++) {
                    Assert.That(expected.Layers[0].Tiles[row, col].ForegroundRed, Is.EqualTo(actual.Layers[0].Tiles[row, col].ForegroundRed));
                }
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetMap_Sets_ForegroundGreen_For_Single_Layer(bool oldFormat)
        {
            var expected = GetSingleLayerTestMap();
            var actual = GetSingleReader(oldFormat).GetMap();
            for (var row = 0; row < expected.Height; row++) {
                for (var col = 0; col < expected.Width; col++) {
                    Assert.That(expected.Layers[0].Tiles[row, col].ForegroundGreen, Is.EqualTo(actual.Layers[0].Tiles[row, col].ForegroundGreen));
                }
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetMap_Sets_ForegroundBlue_For_Single_Layer(bool oldFormat)
        {
            var expected = GetSingleLayerTestMap();
            var actual = GetSingleReader(oldFormat).GetMap();
            for (var row = 0; row < expected.Height; row++) {
                for (var col = 0; col < expected.Width; col++) {
                    Assert.That(expected.Layers[0].Tiles[row, col].ForegroundBlue, Is.EqualTo(actual.Layers[0].Tiles[row, col].ForegroundBlue));
                }
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetMap_Sets_BackgroundRed_For_Single_Layer(bool oldFormat)
        {
            var expected = GetSingleLayerTestMap();
            var actual = GetSingleReader(oldFormat).GetMap();
            for (var row = 0; row < expected.Height; row++) {
                for (var col = 0; col < expected.Width; col++) {
                    Assert.That(actual.Layers[0].Tiles[row, col].BackgroundRed, Is.EqualTo(expected.Layers[0].Tiles[row, col].BackgroundRed), "row " + row + " col " + col);
                }
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetMap_Sets_BackgroundGreen_For_Single_Layer(bool oldFormat)
        {
            var expected = GetSingleLayerTestMap();
            var actual = GetSingleReader(oldFormat).GetMap();
            for (var row = 0; row < expected.Height; row++) {
                for (var col = 0; col < expected.Width; col++) {
                    Assert.That(actual.Layers[0].Tiles[row, col].BackgroundGreen, Is.EqualTo(expected.Layers[0].Tiles[row, col].BackgroundGreen));
                }
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GetMap_Sets_BackgroundBlue_For_Single_Layer(bool oldFormat)
        {
            var expected = GetSingleLayerTestMap();
            var actual = GetSingleReader(oldFormat).GetMap();
            for (var row = 0; row < expected.Height; row++) {
                for (var col = 0; col < expected.Width; col++) {
                    Assert.That(actual.Layers[0].Tiles[row, col].BackgroundBlue, Is.EqualTo(expected.Layers[0].Tiles[row, col].BackgroundBlue), "row " + row + " col " + col);
                }
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void RexReader_Disposes(bool oldFormat)
        {
            var reader = GetSingleReader(oldFormat);
            reader.Dispose();
            Assert.Throws<ObjectDisposedException>(() => reader.GetMap());
            Assert.Throws<ObjectDisposedException>(() => reader.GetLayerCount());
            Assert.Throws<ObjectDisposedException>(() => reader.GetLayerWidth(0));
            Assert.Throws<ObjectDisposedException>(() => reader.GetLayerHeight(0));
            Assert.Throws<ObjectDisposedException>(() => reader.ReadLayerAsString(0));
        }
    }
}
