using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;

namespace Base_Building_Game
{
    public static partial class General
    {
        public class Path : Building
        {
            public Func<Tile, bool> ValidTiles { get; } = (Tile tile) => tile.ID == (short)TileID.Grass;



            public short ID { get; } = (short)BuildingID.Path;
            public Inventory? inventory { get; set; } = null;



            public int MaxHealth { get => int.MaxValue; } // edit this init
            public int CurrentHealth { get => int.MaxValue; set { } }

            public int xSize { get; } = 1;
            public int ySize { get; } = 1;

            public Vector2 pos { get; set; }
            public int rotation { get; set; } = 0;
            public bool rotatable { get; } = false;

            public Path(IVect pos)
            {
                this.pos = pos;
            }
        }
    }
}
