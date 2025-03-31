using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;



namespace Base_Building_Game
{
    public static partial class General
    {
        public class LargePort : Building
        {
            public Func<Tile, bool> ValidTiles { get; } = (Tile tile) => PrivateValidTiles(tile);

            static bool PrivateValidTiles(Tile tile)
            {
                if (tile.ID != (short)TileID.Ocean) { return false; }

                return true;
            }






            public short ID { get; } = (short)BuildingID.LargePort;
            public Inventory? inventory { get; set; } = new Inventory();

            public int MaxHealth { get => Research[ID] * 1000 + 1000; } // edit this init
            public int CurrentHealth { get; set; }

            public int xSize { get; } = 5;
            public int ySize { get; } = 5;

            public Vector2 pos { get; set; }
            public int rotation { get; set; } = 0;
            public bool rotatable { get; } = true;

            public bool friendly { get; set; }

            public LargePort(IVect pos, int rotation, bool friendly = true)
            {
                this.pos = pos;
                this.friendly = friendly;
                this.rotation = rotation;
            }
        }
    }
}
