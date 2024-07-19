using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;

namespace Base_Building_Game
{
    public static partial class General
    {
        public class WorkCamp : Building
        {
            public Inventory? inventory { get; set; }
            private static TileID[] validTileArray = { TileID.Grass, TileID.Sand };

            public Func<Tile, bool> ValidTiles { get; } = (Tile tile) =>
            {
                return Array.IndexOf(validTileArray, (TileID)tile.ID) == -1 ? false : true;
            };

            public int CurrentHealth { get; set; }

            public short ID { get; } = (short)BuildingID.WorkCamp;

            public int xSize { get; } = 1;
            public int ySize { get; } = 1;

            public IVect pos { get; set; }
            public int rotation { get; set; }
            public bool rotatable { get; } = false;

            public Men man { get; }

#pragma warning disable CS8618 // non nullable man should be defined wah wah, it doesnt matter if they are at int.MinValue
                               // or maybe that will cause an issue, we will see
            public WorkCamp(IVect pos)
            {
                this.pos = pos;
                if (pos.X != int.MinValue)
                {
                    Men man = new Men(this.pos, this);
                    this.man = man;
                }
               
            }
        }
    }

    
}
