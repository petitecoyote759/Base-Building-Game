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
        const float OuterWidth = 50;
        const float FallOff = 20;






        public static void CreateLand(IVect[] seeds, Sector sector)
        {
            PerlinMap map = new PerlinMap();

            for (int x = 0; x < SectorSize; x++)
            {
                for (int y = 0; y < SectorSize; y++)
                {



                    float value = 2 * map.GetValue(x, y) / map.PerlinWidth;




                    
                    foreach (IVect seed in seeds)
                    {
                        float dist = RoughSum(seed, new IVect(x, y));
                        if (dist < 500)
                        {
                            value += 5f / (1 + MathF.Pow(2, dist / 30));
                        }
                    }


                    #region Falloff
                    if (x < OuterWidth)
                    {
                        value -= (x - OuterWidth) * (x - OuterWidth) / (FallOff * FallOff); 
                    }
                    else if (x > SectorSize - OuterWidth)
                    {
                        value -= (SectorSize - (x + OuterWidth)) * (SectorSize - (x + OuterWidth)) / (FallOff * FallOff);
                    }

                    if (y < OuterWidth)
                    {
                        value -= (y - OuterWidth) * (y - OuterWidth) / (FallOff * FallOff);
                    }
                    else if (y > SectorSize - OuterWidth)
                    {
                        value -= (SectorSize - (y + OuterWidth)) * (SectorSize - (y + OuterWidth)) / (FallOff * FallOff);
                    }
                    #endregion Falloff


                    #region SelectTileType
                    if (value < -0.2f)
                    {
                        sector[x, y] = new Tile(TileID.DeepOcean);
                    }
                    else if (value < 1.2f)
                    {
                        sector[x, y] = new Tile(TileID.Ocean);
                    }
                    else if (value < 1.4f)
                    {
                        sector[x, y] = new Tile(TileID.Sand);
                    }
                    else
                    {
                        sector[x, y] = new Tile(TileID.Grass);
                    }
                    #endregion SelectTileType
                }
            }
            AddLog("Terrain Generated", ShortDebugger.Priority.DEBUG);
            GenResources(sector);
            AddLog("Resources Generated", ShortDebugger.Priority.DEBUG);
        }



        public static int RoughSum(IVect left, IVect right)
        {
            return Math.Abs(left.x - right.x) + Math.Abs(left.y - right.y);
        }




        #region ResourceNodes
        public static void GenResources(Sector sector)
        {
            GenResourceNode(sector, 8, 1.6f, TileID.Diamond, 600);
            GenResourceNode(sector, 8, 1.4f, TileID.Iron, 300);
            GenResourceNode(sector, 8, 1.3f, TileID.Stone, 0);
            GenResourceNode(sector, 16, 1.2f, TileID.Wood, 0);
        }




#pragma warning disable CS8602 // sector wont be null.
        public static void GenResourceNode(Sector sector, int size, float scale, TileID target, int mindistance)
        {
            PerlinMap Map = new PerlinMap(size);

            for (int x = 0; x < SectorSize; x++)
            {
                for (int y = 0; y < SectorSize; y++)
                {
                    float value = 2 * Map.GetValue(x, y) / Map.PerlinWidth;

                    if (value > scale && sector[x, y].ID == (short)TileID.Grass)
                    {
                        if (new IVect(x - SectorSize / 2, y - SectorSize / 2).Mag() > mindistance)
                        {
                            sector[x, y] = new Tile(target);
                        }
                    }
                }
            }
        }
#pragma warning restore CS8602
        #endregion ResourceNodes
    }
}
