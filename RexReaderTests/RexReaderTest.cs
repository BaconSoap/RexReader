using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RexReader;
namespace RexReaderTests {
    [TestFixture]
    public class RexReaderTest {

        private const string SingleLayer = "data\\singleLayer.xp";
        private const string SingleLayerString = "uA                                                                                                 e";
        private const int SingleLayerWidth = 10;
        private const int SingleLayerHeight = 10;
        private const int SingleLayerLayerCount = 1;
        private Stream SingleLayerStream { get; set; }
        public RexReader.RexReader SingleReader { get { return new RexReader.RexReader(SingleLayerStream); } }

        [SetUp]
        public void SetupStream() {
            var memoryStream = new MemoryStream();
            using (var filestream = new FileStream(SingleLayer, FileMode.Open)) {
                filestream.CopyTo(memoryStream);
            }
            SingleLayerStream = memoryStream;
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

    }
}
