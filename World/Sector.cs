using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        public class Sector
        {
            public Tile[,] map;


            public Tile this[int x, int y] => map[x, y];



            public Sector()
            {
                map = new Tile[SectorSize, SectorSize];
                for (int x = 0; x < SectorSize; x++)
                {
                    for (int y = 0; y < SectorSize; y++)
                    {
                        map[x, y] = new Tile();
                    }
                }
            }
        }
    }
}
