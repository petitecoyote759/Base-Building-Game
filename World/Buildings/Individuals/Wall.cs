using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        public class Wall : Building
        {
            public Func<Tile, bool> ValidTiles { get; } = (Tile tile) => tile.ID == (short)TileID.Grass;



            public short ID { get; } = (short)BuildingID.Wall;
            public Inventory? inventory { get; set; }
            public int CurrentHealth { get; set; }
            // maybe save connections?

            public int xSize { get; } = 1;
            public int ySize { get; } = 1;
        }
    }
}
