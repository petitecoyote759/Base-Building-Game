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
        public class DropPod : Building
        {
            public Func<Tile, bool> ValidTiles { get; } = (Tile tile) => tile.ID == (short)TileID.Grass;



            public short ID { get; } = (short)BuildingID.DropPod;
            public Inventory? inventory { get; set; } = new Inventory();


            public int MaxHealth { get => Research[ID] * 1000 + 1000; } // edit this init
            public int CurrentHealth { get; set; }

            public int xSize { get; } = 3;
            public int ySize { get; } = 3;

            public Vector2 pos { get; set; }

            public int rotation { get; set; } = 0;
            public bool rotatable { get; } = false;

            public bool friendly { get; set; }

            public DropPod(IVect pos, bool friendly = true)
            {
                this.pos = pos;
                this.friendly = friendly;
                inventory = new Inventory();
            }
        }
    }
}
