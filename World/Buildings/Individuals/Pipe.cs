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
                //code checks surrounding tiles and checks if they are pipes or fluid extractors
                //if they are then check if they are the same type fluid as pipe
                //or they can take the type of a neighbouring pipe 
                //if fluids are same type they will start take mean of nearby containers
                //can make more effecient if i have a way to check if a building inherits fluid containers
                //should also make container take mean of all nearby pipes at once rather than one at a time
                //lots of comments but i will forget this because sleepy >.<


                //up
                Tile up = world.GetTile(pos.X, pos.Y - 1);
                if (up.building != null && up.building.ID == (short)BuildingID.Pipe)
                {
                    if (type == 0 && up.building.type != 0)
                    {
                        type = up.building.type;
                    }
                    if (type == up.building.type)
                    {
                        amount = (amount + up.building.amount) / 2;
                        up.building.amount = amount;

                    }
                }


                else if (up.building != null && up.building.ID == (short)BuildingID.WaterPump) 
                {
                    if (type == 0) 
                    {
                        type = 1;
                    }
                    if (type == 1) 
                    {
                        amount = (amount + up.building.amount) / 2;
                        up.building.amount = amount;

                    }

                }


                else if (up.building != null && up.building.ID == (short)BuildingID.OilRig)
                {
                    if (type == 0)
                    {
                        type = 2;
                    }
                    if (type == 2)
                    {
                        amount = (amount + up.building.amount) / 2;
                        up.building.amount = amount;

                    }

                }


                //right
                Tile right = world.GetTile(pos.X + 1, pos.Y);
                if (right.building != null &&  right.building.ID == (short)BuildingID.Pipe) 
                {
                    if (type == 0 && right.building.type != 0)
                    {
                        type = right.building.type;
                    }
                    if (type == right.building.type)
                    {
                        amount = (amount + right.building.amount) / 2;
                        right.building.amount = amount;

                    }
                }


                else if (right.building != null && right.building.ID == (short)BuildingID.WaterPump)
                {
                    if (type == 0)
                    {
                        type = 1;
                    }
                    if (type == 1)
                    {
                        amount = (amount + right.building.amount) / 2;
                        right.building.amount = amount;

                    }

                }


                else if (right.building != null && right.building.ID == (short)BuildingID.OilRig)
                {
                    if (type == 0)
                    {
                        type = 2;
                    }
                    if (type == 2)
                    {
                        amount = (amount + right.building.amount) / 2;
                        right.building.amount = amount;

                    }

                }


                //down
                Tile down = world.GetTile(pos.X, pos.Y + 1);
                if (down.building != null && down.building.ID == (short)BuildingID.Pipe) 
                {
                    if (type == 0 && down.building.type != 0)
                    {
                        type = down.building.type;
                    }
                    if (type == down.building.type)
                    {
                        amount = (amount + down.building.amount) / 2;
                        down.building.amount = amount;

                    }
                }


                else if (down.building != null && down.building.ID == (short)BuildingID.WaterPump)
                {
                    if (type == 0)
                    {
                        type = 1;
                    }
                    if (type == 1)
                    {
                        amount = (amount + down.building.amount) / 2;
                        down.building.amount = amount;

                    }

                }


                else if (down.building != null && down.building.ID == (short)BuildingID.OilRig)
                {
                    if (type == 0)
                    {
                        type = 2;
                    }
                    if (type == 2)
                    {
                        amount = (amount + down.building.amount) / 2;
                        down.building.amount = amount;

                    }

                }


                //left
                Tile left = world.GetTile(pos.X - 1, pos.Y);
                if (left.building != null && left.building.ID == (short)BuildingID.Pipe)
                {
                    if (type == 0 && left.building.type != 0)
                    {
                        type = left.building.type;
                    }
                    if (type == left.building.type)
                    {
                        amount = (amount + left.building.amount) / 2;
                        left.building.amount = amount;

                    }
                }


                else if (left.building != null && left.building.ID == (short)BuildingID.WaterPump)
                {
                    if (type == 0)
                    {
                        type = 1;
                    }
                    if (type == 1)
                    {
                        amount = (amount + left.building.amount) / 2;
                        left.building.amount = amount;

                    }

                }


                else if (left.building != null && left.building.ID == (short)BuildingID.OilRig)
                {
                    if (type == 0)
                    {
                        type = 2;
                    }
                    if (type == 2)
                    {
                        amount = (amount + left.building.amount) / 2;
                        left.building.amount = amount;

                    }

                }
            }

            public Pipe(IVect pos)
            {
                this.pos = pos;
            }

        }


    }
}
