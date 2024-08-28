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

            public float amount { get; set; } = 0;

            public short type { get; set; } = (short)FluidID.None;

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
                //checks if building nearby are fluid containers
                //gets mean amount of all nearby fluid containers and makes all amounts equal to mean
                //Have not tested but made code smaller, commented out section is version using mean


                //up
                Tile up = world.GetTile(pos.X, pos.Y - 1);
                Tile right = world.GetTile(pos.X + 1, pos.Y);
                Tile down = world.GetTile(pos.X, pos.Y + 1);
                Tile left = world.GetTile(pos.X - 1, pos.Y);
                Tile self = world.GetTile(pos.X, pos.Y);
                List<FluidContainer> all = new List<FluidContainer>();

                FluidContainer upcontainer = (FluidContainer)self.building;
                FluidContainer rightcontainer = (FluidContainer)self.building;
                FluidContainer downcontainer = (FluidContainer)self.building;
                FluidContainer leftcontainer = (FluidContainer)self.building;
                FluidContainer selfcontainer = (FluidContainer)self.building;

                if (up.building != null) 
                {
                    if (up.building is Linker uplinker && uplinker.ID == (short)BuildingID.OilRig)
                    {
                        upcontainer = (FluidContainer)uplinker.connectedBuilding;
                    }
                    else 
                    {
                        upcontainer = (FluidContainer)up.building;
                    }
                }
                if (upcontainer == selfcontainer) 
                {

                    upcontainer = null;
                }
                
                

                if (upcontainer != null)
                {
                    if (type == (short)FluidID.None && upcontainer.type != (short)FluidID.None)
                    {
                        type = upcontainer.type;
                    }
                    if (type == upcontainer.type)
                    {
                        all.Add(upcontainer);
                    }
                    
                }


                //right

                if (right.building != null)
                {
                    if (right.building is Linker rightlinker && rightlinker.ID == (short)BuildingID.OilRig)
                    {
                        rightcontainer = (FluidContainer)rightlinker.connectedBuilding;
                    }
                    else
                    {
                        rightcontainer = (FluidContainer)right.building;
                    }
                }
                if (rightcontainer == selfcontainer)
                {
                    rightcontainer = null;
                }


                if (rightcontainer != null)
                {
                    if (type == (short)FluidID.None && rightcontainer.type != (short)FluidID.None)
                    {
                        type = rightcontainer.type;
                    }
                    if (type == rightcontainer.type)
                    {
                        all.Add(rightcontainer);
                    }

                }

                //down
                if (down.building != null)
                {
                    if (down.building is Linker downlinker && downlinker.ID == (short)BuildingID.OilRig)
                    {
                        downcontainer = (FluidContainer)downlinker.connectedBuilding;
                    }
                    else
                    {
                        downcontainer = (FluidContainer)down.building;
                    }
                }
                if (downcontainer == selfcontainer)
                {
                    downcontainer = null;
                }


                if (downcontainer != null)
                {
                    if (type == (short)FluidID.None && downcontainer.type != (short)FluidID.None)
                    {
                        type = downcontainer.type;
                    }
                    if (type == downcontainer.type)
                    {
                        all.Add(downcontainer);
                    }
                }

                //left

                if (left.building != null)
                {
                    if (left.building is Linker leftlinker && leftlinker.ID == (short)BuildingID.OilRig)
                    {
                        leftcontainer = (FluidContainer)leftlinker.connectedBuilding;
                    }
                    else
                    {
                        leftcontainer = (FluidContainer)left.building;
                    }
                }
                if (leftcontainer == selfcontainer)
                {
                    leftcontainer = null;
                }


                if (leftcontainer != null)
                {
                    if (type == (short)FluidID.None && leftcontainer.type != (short)FluidID.None)
                    {
                        type = leftcontainer.type;
                    }
                    if (type == leftcontainer.type)
                    {
                        all.Add(leftcontainer);
                    }
                }

                //all amount change

                if (all.Count != 0)
                {
                    Console.WriteLine("waaa");
                    float temp = 0;
                    all.Add(selfcontainer);
                    for (int i = 0; i < all.Count; i++) 
                    {
                        temp += all[i].amount;
                    }
                    temp = temp / (float)all.Count;
                    for (int i = 0; i < all.Count; i++) 
                    {

                        all[i].amount = temp;

                    }


                }
                Console.WriteLine(amount);

            }

            public Pipe(IVect pos)
            {
                this.pos = pos;
            }

        }


    }
}
