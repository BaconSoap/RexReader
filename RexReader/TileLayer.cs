namespace Varnerin.RexTools {
    public class TileLayer {
        /// <summary>
        /// A 2D array of the tiles in this layer, of the form Tiles[y,x].
        /// Tiles[0,10] is the tile in the first row and the eleventh column.
        /// </summary>
        public Tile[,] Tiles { get; set; }

        public TileLayer(int width, int height) {
            //Going by y,x is low-hanging fruit on the tree of optimization
            Tiles = new Tile[height, width];
        }
    }
}
