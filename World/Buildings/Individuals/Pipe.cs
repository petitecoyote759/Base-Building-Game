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
        public class Pipe : FBuilding, ConnectingBuilding, FluidContainer
        {
            public Func<Tile, bool> ValidTiles { get; } = (Tile tile) =>
            {
                return tile.ID == (short)TileID.Grass || tile.ID == (short)TileID.Sand; // use a array to make more effic
            };

            public short ID { get; } = (short)BuildingID.Pipe;

            public int xSize { get; } = 1;
            public int ySize { get; } = 1;

            public int amount { get; set; } = 0;

            public short type { get; set; } = 0;

            public Inventory? inventory { get; set; } = new Inventory();



            public int MaxHealth { get => Research[ID] * 1000 + 1000; } // edit this init
            public int CurrentHealth { get; set; }

            public Vector2 pos { get; set; }
            public int rotation { get; set; } = 0;
            public bool rotatable { get; } = false;

            public Func<Tile, bool> Connections { get; } = (Tile tile) => tile.building is not null && tile.building.ID == (short)BuildingID.Pipe;

            public IntPtr connectionImage
            {
                get => renderer.images["PipeSegment" + Research[ID]];
            }

            public void Action(int dt)
            {
                //type 0 = none
                //type 1 = water
                //type 2 = oil
                //checks if building nearby are fluid containers
                //gets mean amount of all nearby fluid containers and makes all amounts equal to mean
                //Have not tested but made code smaller, commented out section is version using mean



                //up
                Tile up = world.GetTile(pos.X, pos.Y - 1);
                Tile right = world.GetTile(pos.X + 1, pos.Y);
                Tile down = world.GetTile(pos.X, pos.Y + 1);
                Tile left = world.GetTile(pos.X - 1, pos.Y);
                List<Tile> all = new List<Tile>();
                

                if (up.building != null && up.building is FluidContainer)
                {
                    if (type == 0 && ((FluidContainer)up.building).type != 0)
                    {
                        type = ((FluidContainer)up.building).type;
                    }
                    if (type == ((FluidContainer)up.building).type)
                    {

                        amount = (amount + ((FluidContainer)up.building).amount) / 2;
                        ((FluidContainer)up.building).amount = amount;
                        //all.Add(up);

                    }
                }

                //right
                
                if (right.building != null &&  right.building is FluidContainer) 
                {
                    if (type == 0 && ((FluidContainer)right.building).type != 0)
                    {
                        type = ((FluidContainer)right.building).type;
                    }
                    if (type == ((FluidContainer)right.building).type)
                    {
                        amount = (amount + ((FluidContainer)right.building).amount) / 2;
                        ((FluidContainer)right.building).amount = amount;
                        //all.Add(right);
                    }
                }

                //down
                
                if (down.building != null && down.building is FluidContainer) 
                {
                    if (type == 0 && ((FluidContainer)down.building).type != 0)
                    {
                        type = ((FluidContainer)down.building).type;
                    }
                    if (type == ((FluidContainer)down.building).type)
                    {
                        amount = (amount + ((FluidContainer)down.building).amount) / 2;
                        ((FluidContainer)down.building).amount = amount;
                        //all.Add(down);
                    }
                }

                //left
                
                if (left.building != null && left.building is FluidContainer)
                {
                    if (type == 0 && ((FluidContainer)left.building).type != 0)
                    {
                        type = ((FluidContainer)left.building).type;
                    }
                    if (type == ((FluidContainer)left.building).type)
                    {
                        amount = (amount + ((FluidContainer)left.building).amount) / 2;
                        ((FluidContainer)left.building).amount = amount;
                        //all.Add(left);
                    }
                }

                //all amount change
                /*
                if (all.Count != 0)
                {
                    int temp = 0;
                    for (int i = 0; i < all.Count; i++) 
                    {
                        temp = ((FluidContainer)all[i].building).amount + temp;
                    }
                    all.Add(world.GetTile(pos.X, pos.Y));
                    temp = temp / all.Count;
                    for (int i = 0; i < all.Count; i++) 
                    {

                        ((FluidContainer)all[i].building).amount = temp;

                    }

                
                }
                */


            }

            public Pipe(IVect pos)
            {
                this.pos = pos;
            }

        }


    }
}
