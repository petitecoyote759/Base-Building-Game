using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;






namespace Base_Building_Game
{
    public static partial class General
    {
        public static void CreateLand(IVect[] seeds, Sector sector)
        {
            PerlinMap map = new PerlinMap();

            for (int x = 0; x < SectorSize; x++)
            {
                for (int y = 0; y < SectorSize; y++)
                {
                    float value = 2 * map.GetValue(x, y) / PerlinMap.PerlinWidth;

                    //foreach (IVect seed in seeds)
                    //{
                    //    value += 1f / (1 + MathF.Pow(2, (seed - new IVect(x, y)).MagSquared()));
                    //}



                    if (value < -0.5f)
                    {
                        sector[x, y] = new Tile(TileID.DeepOcean);
                    }
                    else if (value < 0.8f)
                    {
                        sector[x, y] = new Tile(TileID.Ocean);
                    }
                    else if (value < 1f)
                    {
                        sector[x, y] = new Tile(TileID.Sand);
                    }
                    else
                    {
                        sector[x, y] = new Tile(TileID.Grass);
                    }
                }
            }


        }
    }
}
