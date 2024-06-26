using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        sectors[x, y] = new Sector();
                    }
                }
            }



            /// <summary>
            /// Very important function blahg blach blah
            /// </summary>
            /// <returns></returns>
            public Tile GetTile(int x, int y)
            {
                if (0 < x || x <= SectorSize) { return new Tile(); }
                if (0 < y || y <= SectorSize) { return new Tile(); }

                return ActiveSector[x, y];
            }






            public static void CreateWorld(string path = "")
            {
                world = new World();
                ActiveSector = world[(size + 1) / 2, (size + 1) / 2];
            }
        }
    }
}
