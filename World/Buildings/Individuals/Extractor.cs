using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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


            const int MaxTime = 8000;
            const int TimePerTier = 1000;
            int TimeSpent = MaxTime;
            byte tier => Research[ID];
            Item? LastItem;

            public IVect pos { get; set; }


            public void Action(int dt)
            {
                TimeSpent -= dt;

                if (TimeSpent < 0)
                {
                    if (LastItem is null)
                    {
                        LastItem = new Item(ItemID.Wood, (pos * 32) + new IVect(16, 16));
                    }
                    else
                    {
                        if (LastItem.pos.x < pos.x * 32 || LastItem.pos.x + 32 < pos.x * 32 ||
                            LastItem.pos.y < pos.y * 32 || LastItem.pos.y + 32 < pos.y * 32)
                        {
                            LastItem.InExtractor = false;
                            LastItem = new Item(ItemID.Wood, (pos * 32) + new IVect(16, 16));
                        }
                    }
                    TimeSpent = MaxTime - (TimePerTier * tier) - TimeSpent;


                }
            }





            public Extractor(IVect pos)
            {
                this.pos = pos;
            }
        }
    }
}
