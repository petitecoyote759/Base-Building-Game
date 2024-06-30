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
        public class Bridge : Building
        {
            public Func<Tile, bool> ValidTiles { get; } = (Tile tile) => tile.ID == (short)TileID.Grass; 



            public short ID { get; } = (short)BuildingID.Bridge;
            public Inventory? inventory { get; set; } 
            public int CurrentHealth { get; set; }

            public int xSize { get; } = 1;
            public int ySize { get; } = 1;

            public IVect pos { get; set; }

            public Bridge(IVect pos)
            {
                this.pos = pos;
            }
        }
    }
}
