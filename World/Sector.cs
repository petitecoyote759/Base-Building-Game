using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Short_Tools.General;

namespace Base_Building_Game
{
    public static partial class General
    {
        public class Sector
        {
            public Tile[,]? map;


            public Tile? this[int x, int y] 
            { 
                get => map is null ? null : map [x, y];
                set => map[x, y] = value;
            }



            public Sector(bool loaded = false)
            {
                if (loaded)
                {
                    map = new Tile[SectorSize, SectorSize];
                    for (int x = 0; x < SectorSize; x++)
                    {
                        for (int y = 0; y < SectorSize; y++)
                        {
                            map[x, y] = new Tile(TileID.Error);
                        }
                    }
                }
                else
                {
                    map = null;
                }
            }
        }
    }
}
