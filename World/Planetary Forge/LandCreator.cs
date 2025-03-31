using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;






namespace Base_Building_Game.WorldGen.LandGen
{
    public static partial class General
    {
        const float OuterWidth = 50;
        const float FallOff = 20;





        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void CreateLand(IVect[] seeds, Base_Building_Game.General.Sector sector)
        {
            Perlin.PerlinMap map = new Perlin.PerlinMap();

            Perlin.PerlinMap subMap1 = new Perlin.PerlinMap(16);

            Perlin.PerlinMap subMap2 = new Perlin.PerlinMap(8);

            float[,] data = new float[Base_Building_Game.General.SectorSize, Base_Building_Game.General.SectorSize];

            for (int x = 0; x < Base_Building_Game.General.SectorSize; x++)
            {
                for (int y = 0; y < Base_Building_Game.General.SectorSize; y++)
                {



                    float value = 2 * map.GetValue(x, y) / map.PerlinWidth;

                    value += 0.5f * subMap1.GetValue(x, y) / subMap1.PerlinWidth;

                    value += 0.25f * subMap2.GetValue(x, y) / subMap2.PerlinWidth;


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
                    else if (x > Base_Building_Game.General.SectorSize - OuterWidth)
                    {
                        value -= (Base_Building_Game.General.SectorSize - (x + OuterWidth)) * (Base_Building_Game.General.SectorSize - (x + OuterWidth)) / (FallOff * FallOff);
                    }

                    if (y < OuterWidth)
                    {
                        value -= (y - OuterWidth) * (y - OuterWidth) / (FallOff * FallOff);
                    }
                    else if (y > Base_Building_Game.General.SectorSize - OuterWidth)
                    {
                        value -= (Base_Building_Game.General.SectorSize - (y + OuterWidth)) * (Base_Building_Game.General.SectorSize - (y + OuterWidth)) / (FallOff * FallOff);
                    }
                    #endregion Falloff


                    data[x, y] = value;
                }
            }




            for (int x = 0; x < Base_Building_Game.General.SectorSize; x++)
            {
                for (int y = 0; y < Base_Building_Game.General.SectorSize; y++)
                {
                    float value = data[x, y];
                    // TODO: try doing some dy/dx stuff to get the uhhhh the uhhhhhhhhhhh cliffs and stuff
                    #region SelectTileType

                    const float cliffGrad = 0.11f;


                    if (0 < x && x < Base_Building_Game.General.SectorSize - 1 &&
                        0 < y && y < Base_Building_Game.General.SectorSize - 1 &&

                        ( // grad detection
                        (Math.Abs(data[x - 1, y]) - Math.Abs(value) > cliffGrad) ||
                        (Math.Abs(data[x + 1, y]) - Math.Abs(value) > cliffGrad) ||
                        (Math.Abs(data[x, y + 1]) - Math.Abs(value) > cliffGrad) ||
                        (Math.Abs(data[x, y - 1]) - Math.Abs(value) > cliffGrad)
                        ) &&
                        data[x, y] > 1.2f // not water init
                        )
                    {
                        sector[x, y] = new Base_Building_Game.General.Tile(Base_Building_Game.General.TileID.Error);
                    }



                    else if (value < -0.2f)
                    {
                        sector[x, y] = new Base_Building_Game.General.Tile(Base_Building_Game.General.TileID.DeepOcean);
                    }
                    else if (value < 1.2f)
                    {
                        sector[x, y] = new Base_Building_Game.General.Tile(Base_Building_Game.General.TileID.Ocean);
                    }
                    else if (value < 1.4f)
                    {
                        sector[x, y] = new Base_Building_Game.General.Tile(Base_Building_Game.General.TileID.Sand);
                    }
                    else
                    {
                        sector[x, y] = new Base_Building_Game.General.Tile(Base_Building_Game.General.TileID.Grass);
                    }
                    #endregion SelectTileType
                }
            }
            Base_Building_Game.General.AddLog("Terrain Generated", ShortDebugger.Priority.DEBUG);
            GenResources(sector);
            Base_Building_Game.General.AddLog("Resources Generated", ShortDebugger.Priority.DEBUG);
        }



        public static int RoughSum(IVect left, IVect right)
        {
            return Math.Abs(left.x - right.x) + Math.Abs(left.y - right.y);
        }




        #region ResourceNodes
        public static void GenResources(Base_Building_Game.General.Sector sector)
        {
            GenResourceNode(sector, 8, 1.6f, Base_Building_Game.General.TileID.Diamond, 600);
            GenResourceNode(sector, 8, 1.4f, Base_Building_Game.General.TileID.Iron, 300);
            GenResourceNode(sector, 8, 1.3f, Base_Building_Game.General.TileID.Stone, 0);
            GenResourceNode(sector, 16, 1.2f, Base_Building_Game.General.TileID.Wood, 0);
        }




#pragma warning disable CS8602 // sector wont be null.
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void GenResourceNode(Base_Building_Game.General.Sector sector, int size, float scale,
            Base_Building_Game.General.TileID target, int mindistance)
        {
            Perlin.PerlinMap Map = new Perlin.PerlinMap(size);

            for (int x = 0; x < Base_Building_Game.General.SectorSize; x++)
            {
                for (int y = 0; y < Base_Building_Game.General.SectorSize; y++)
                {
                    float value = 2 * Map.GetValue(x, y) / Map.PerlinWidth;

                    if (value > scale && sector[x, y].ID == (short)Base_Building_Game.General.TileID.Grass)
                    {
                        if (new IVect(x - Base_Building_Game.General.SectorSize / 2, 
                            y - Base_Building_Game.General.SectorSize / 2).Mag() > mindistance)
                        {
                            sector[x, y] = new Base_Building_Game.General.Tile(target);
                        }
                    }
                }
            }
        }
#pragma warning restore CS8602
        #endregion ResourceNodes
    }
}
