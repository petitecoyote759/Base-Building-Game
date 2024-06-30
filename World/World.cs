using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;
using static Short_Tools.General;




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



            /// <summary>
            /// Very important function blahg blach blah
            /// </summary>
            /// <returns></returns>
            public Tile GetTile(int x, int y)
            {
                if (0 > x || x >= SectorSize) { return new Tile(TileID.Error); }
                if (0 > y || y >= SectorSize) { return new Tile(TileID.Error); }

                return ActiveSector[x, y];
            }



            public bool Walkable(Tile tile, bool player = false)
            {
                if (settings.Cheats && player) { return true; } 

                if (tile.ID == (short)TileID.DeepOcean ||
                    tile.ID == (short)TileID.Ocean) 
                { return false; }

                if (tile.building is not null)
                {
                    if (tile.building.ID == (short)BuildingID.Wall) { return false; }
                }

                return true;
            }
            public bool Walkable(int x, int y, bool player = false)
            {
                return Walkable(GetTile(x, y), player);
            }
            public bool Walkable(IVect pos, bool player = false)
            {
                return Walkable(pos.x, pos.y, player);
            }
        }
    }
}
