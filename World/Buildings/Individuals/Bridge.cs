using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
            public Func<Tile, bool> ValidTiles { get; } = (Tile tile) => tile.ID == (short)TileID.Ocean; 



            public short ID { get; } = (short)BuildingID.Bridge;
            public Inventory? inventory { get; set; } = null;



            public int MaxHealth { get => Research[ID] * 1000 + 1000; } // edit this init
            public int CurrentHealth { get; set; }

            public int xSize { get; } = 1;
            public int ySize { get; } = 1;

            public Vector2 pos { get; set; }
            public int rotation { get; set; } = 0;
            public bool rotatable { get; } = false;

            public Bridge(IVect pos)
            {
                this.pos = pos;
            }
        }
    }
}
