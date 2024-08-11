using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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



            public int MaxHealth { get => Research[ID] * 1000 + 1000; } // edit this init
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

            public Vector2 pos { get; set; }
            public int rotation { get; set; } = 0;
            public bool rotatable { get; } = false;
            short OnTileID = 0;
            short ItemDropID = 0;

            public void Action(int dt)
            {
                TimeSpent -= dt;

                if (TimeSpent < 0)
                {
                    if (LastItem is null)
                    {
                        LastItem = new Item(ItemDropID, (Vector2)(pos));
                    }
                    else
                    {
                        if (LastItem.pos.X < pos.X || LastItem.pos.X + 1 < pos.X ||
                            LastItem.pos.Y < pos.Y || LastItem.pos.Y + 1 < pos.Y)
                        {
                            LastItem.InExtractor = false;
                            LastItem = new Item(ItemDropID, (Vector2)(pos));
                        }
                    }
                    TimeSpent = MaxTime - (TimePerTier * tier) - TimeSpent;


                }
            }





            public Extractor(IVect pos)
            {
                this.pos = pos;
                OnTileID = world.GetTile(pos.x, pos.y).ID;
                if (TileIDToItem.ContainsKey(OnTileID))
                {
                    ItemDropID = TileIDToItem[OnTileID];
                }
            }
        }
    }
}
