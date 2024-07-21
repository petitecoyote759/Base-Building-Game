using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;
using static Short_Tools.General;
using System.Xml.Serialization;




namespace Base_Building_Game
{
    public static partial class General
    {
        public class World
        {
            public Sector[,] sectors;
            public static int size = 9; // how many sectors x and y the map is



            public Sector this[int x, int y] => sectors[x, y];



            public World()
            {
                sectors = new Sector[size, size];

                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        sectors[x, y] = new Sector(false);
                    }
                }
            }


            #region Tile Getters

#pragma warning disable CS8603 // Active sector being null, no its not init.

            /// <summary>
            /// Very important function blahg blach blah
            /// </summary>
            /// <returns></returns>
            public Tile GetTile(int x, int y)
            {
                if (0 > x || x >= SectorSize) { return new Tile(TileID.DeepOcean); }
                if (0 > y || y >= SectorSize) { return new Tile(TileID.DeepOcean); }

                return ActiveSector[x, y];
            }


            /// <summary>
            /// Very important function blahg blach blah
            /// </summary>
            /// <returns></returns>
            public Tile GetTile(float x, float y)
            {
                if (0 > x || x >= SectorSize) { return new Tile(TileID.DeepOcean); }
                if (0 > y || y >= SectorSize) { return new Tile(TileID.DeepOcean); }

                return ActiveSector[(int)x, (int)y];
            }


#pragma warning restore CS8603

            #endregion Tile Getters


            public bool Walkable(int x, int y, bool player = false)
            {
                Tile tile = world.GetTile(x, y);

                if (settings.Cheats && player) { return true; } 


                if (tile.ID == (short)TileID.DeepOcean) { return false; }


                if (tile.ID == (short)TileID.Ocean) 
                {
                    if (tile.building is not null)
                    {
                        if (tile.building.ID == (short)BuildingID.Bridge ||
                            tile.building.ID == (short)BuildingID.SmallPort ||
                            tile.building.ID == (short)BuildingID.MedPort ||
                            tile.building.ID == (short)BuildingID.LargePort)
                        {
                            return true;
                        }
                    }



                    if (IsOnBoat(x, y)) { return true; }
                    return false;
                }

                if (tile.building is not null)
                {
                    if (tile.building.ID == (short)BuildingID.Wall) { return false; }
                }

                return true;
            }


            public bool Walkable(IVect pos, bool player = false)
            {
                return Walkable(pos.x, pos.y, player);
            }
        }
    }
}
