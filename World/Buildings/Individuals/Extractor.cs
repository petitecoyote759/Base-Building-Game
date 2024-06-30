using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;




namespace Base_Building_Game
{
    public static partial class General
    {
        public class Extractor : FBuilding
        {
            private static short[] Tiles = new short[]
            {
                (short)TileID.Wood, (short)TileID.Stone,
                (short)TileID.Iron, (short)TileID.Diamond,
            };

            public Func<Tile, bool> ValidTiles { get; } = (Tile tile) => 
            (Tiles.Contains(tile.ID));

            public int CurrentHealth { get; set; } = 1000;
            public short ID { get; } = (short)BuildingID.Extractor;

            public Inventory? inventory { get; set; } = null;

            public int xSize { get; } = 1;
            public int ySize { get; } = 1;




            public void Action(int dt)
            {

            }
        }
    }
}
