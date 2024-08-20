using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Base_Building_Game.General;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;

namespace Base_Building_Game
{
    public static partial class General
    {
        public class WaterPump : FBuilding, FluidContainer
        {
            public Func<Tile, bool> ValidTiles { get; } = (Tile tile) => tile.ID == (short)TileID.Ocean;

            public short ID { get; } = (short)BuildingID.WaterPump;

            public int xSize { get; } = 1;
            public int ySize { get; } = 1;

            public float amount { get; set; } = 0;

            public short type { get; set; } = 1;



            public Inventory? inventory { get; set; } = new Inventory();



            public int MaxHealth { get => Research[ID] * 1000 + 1000; } // edit this init
            public int CurrentHealth { get; set; }

            public Vector2 pos { get; set; }
            public int rotation { get; set; } = 0;
            public bool rotatable { get; } = false;

            public WaterPump(IVect pos)
            {
                this.pos = pos;
            }

            public void Action(int dt) 
            {
                amount = amount + 5;
                
            }

        }



    }
}
