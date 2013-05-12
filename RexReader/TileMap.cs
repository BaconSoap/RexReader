using System.Linq;

namespace Varnerin.RexTools {
    public class TileMap {
        public TileLayer[] Layers { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int LayerCount { get { return Layers.Count(); } }

        public TileMap(int width, int height, int layers) {
            Layers = new TileLayer[layers];
            Width = width;
            Height = height;
            for (var i = 0; i < layers; i++) {
                Layers[i] = new TileLayer(width, height);
            }
        }
    }
}
