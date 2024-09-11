using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;
using Sector = Base_Building_Game.General.Sector;
using BuildingID = Base_Building_Game.General.BuildingID;



namespace PlanetaryForge
{
    public interface WeightedItem
    {
        public int weight { get; }
    }



    public class WeightedRandomiser<T> where T : WeightedItem
    {
        T[] values;
        int[] weights;
        int totalWeight = 0;

        public WeightedRandomiser(T[] values)
        {
            this.values = values;

            weights = new int[values.Length];
            weights[0] = values[0].weight;
            for (int i = 1; i < values.Length; i++)
            {
                weights[i] = weights[i - 1] + values[i].weight;
            }
            totalWeight = weights.Last();
        }

        public T GetRandomValue(Random? random = null)
        {
            random ??= new Random();
            int value = random.Next(totalWeight + 1);
            for (int i = 0; i < weights.Length; i++)
            {
                if (weights[i] > value) { return values[i]; }
            }
            return values.Last();
        }
    }










    internal class Building : WeightedItem
    {
        public int weight { get; } = 1;

        internal short[,] data;
        private int w;
        private int h;


        internal AccessPoint[] points;


        private static bool IsPath(short value)
        {
            return value == 1;
        }


        internal Building(short[,] data, int weight)
        {
            this.data = data;
            this.weight = weight;

            w = data.GetLength(0);
            h = data.GetLength(1);

            CalcAccessPoints();
        }

        private void CalcAccessPoints()
        {
            List<AccessPoint> points = new List<AccessPoint>();
            IVect TestPoint = new IVect(0, 0);

            for (int x = 0; x < w; x++)
            {
                TestPoint.x = x;
                if (IsPath(Get(data, TestPoint))) { points.Add(new AccessPoint() { pos = TestPoint, dir = new IVect(0, -1) }); }
            }

            for (int y = 0; y < w; y++)
            {
                TestPoint.y = y;
                if (IsPath(Get(data, TestPoint))) { points.Add(new AccessPoint() { pos = TestPoint, dir = new IVect(1, 0) }); }
            }

            for (int x = w - 2; x >= 0; x--)
            {
                TestPoint.x = x;
                if (IsPath(Get(data, TestPoint))) { points.Add(new AccessPoint() { pos = TestPoint, dir = new IVect(0, 1) }); }
            }

            for (int y = h - 2; y >= 0; y--)
            {
                TestPoint.y = y;
                if (IsPath(Get(data, TestPoint))) { points.Add(new AccessPoint() { pos = TestPoint, dir = new IVect(-1, 0) }); }
            }

            this.points = points.ToArray();
        }



        internal void Build(IVect pos, Action<int, int, short> Place)
        {
            //// Instantly bricks the game, huh
            //for (int x = -1; x < w + 1; x++)
            //{
            //    for (int y = -1; y < h + 1; y++)
            //    {
            //        Place(pos.x + x, pos.y + y, 6); // wall
            //    }
            //}

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Place(pos.x + x, pos.y + y, data[x, y]);
                }
            }
        }


        internal bool CanBuild(AccessPoint point, IVect buildDest, Func<int, int, bool> Buildable)
        {
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (!Buildable(
                        buildDest.x - point.pos.x + x,
                        buildDest.y - point.pos.y + y))
                    {
                        return false;
                    }
                }
            }
            return true;
        }












        private static short Get(short[,] data, IVect pos) => data[pos.x, pos.y];
    }




    internal struct AccessPoint
    {
        internal IVect pos;
        internal IVect dir;

        public override string ToString()
        {
            return $"({pos}|{dir})";
        }
    }






















    internal class General
    {
        static Sector sector;

        public static void Run(IVect seed, Sector inSector)
        {
            sector = inSector;
            // TODO: move to json files
            Building[] buildings = new Building[] {

            #region MainArea
            new Building(new short[,] {
            { 6, 6, 6, 1, 6, 6, 6 },
            { 6, 1, 1, 1, 1, 1, 6 },
            { 6, 1, 2, 0, 0, 1, 6 },
            { 1, 1, 0, 0, 0, 1, 1 },
            { 6, 1, 0, 0, 0, 1, 6 },
            { 6, 1, 1, 1, 1, 1, 6 },
            { 6, 6, 6, 1, 6, 6, 6 },
            }, 0),
                #endregion

            #region CrossRoads
            new Building(new short[,] {
            { 0, 1, 0 },
            { 1, 1, 1 },
            { 0, 1, 0 }
            }, 3),
            #endregion

            #region Straights
            new Building(new short[,] {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 0, 1, 0 }
            }, 4),

            new Building(new short[,] {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 }
            }, 4),
            #endregion

            #region Deadends
                new Building(new short[,] {
            { 0, 1, 0 },
            { 0, 0, 0 },
            { 0, 0, 0 }
            }, 2),

            new Building(new short[,] {
            { 0, 0, 0 },
            { 0, 0, 1 },
            { 0, 0, 0 }
            }, 2),

            new Building(new short[,] {
            { 0, 0, 0 },
            { 0, 0, 0 },
            { 0, 1, 0 }
            }, 2),

            new Building(new short[,] {
            { 0, 0, 0 },
            { 1, 0, 0 },
            { 0, 0, 0 }
            }, 2),
                #endregion

            #region TJunctions
            new Building(new short[,] {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 0, 1, 0 }
            }, 2),

            new Building(new short[,] {
            { 0, 1, 0 },
            { 1, 1, 0 },
            { 0, 1, 0 }
            }, 2),

            new Building(new short[,] {
            { 0, 1, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 }
            }, 2),

            new Building(new short[,] {
            { 0, 1, 0 },
            { 0, 1, 1 },
            { 0, 1, 0 }
            }, 2),
            #endregion
            };


            GenVillage(buildings, seed);
        }







        static int maxBuildAttempts = 10;

        private static void GenVillage(Building[] buildings, IVect seed)
        {
            WeightedRandomiser<Building> randy = new WeightedRandomiser<Building>(buildings);
            int currentBuildAttempt = 0;

            buildings[0].Build(seed, Place);

            Queue<AccessPoint> nodesToVisit = new Queue<AccessPoint>();
            foreach (AccessPoint point in buildings[0].points)
            {
                nodesToVisit.Enqueue(new AccessPoint() { pos = point.pos + seed, dir = point.dir });
            }


            while (nodesToVisit.Count() != 0)
            {
                currentBuildAttempt = 0;
                AccessPoint node = nodesToVisit.Dequeue();

                IVect testPos = node.pos + node.dir;

            SelectBuilding:
                Building building = randy.GetRandomValue();

                bool built = false;
                foreach (AccessPoint point in building.points)
                {
                    if (IVect.Dot(point.dir, node.dir) == -1)
                    {
                        if (building.CanBuild(point, testPos, Buildable))
                        {
                            built = true;
                            building.Build(testPos - point.pos, Place);

                            foreach (AccessPoint addPoint in building.points)
                            {
                                nodesToVisit.Enqueue(new AccessPoint() { pos = addPoint.pos + testPos - point.pos, dir = addPoint.dir });
                            }
                        }
                    }
                }

                if (!built && currentBuildAttempt < maxBuildAttempts)
                {
                    currentBuildAttempt++;
                    goto SelectBuilding;
                }
            }
        }



        enum BuildingEnum : short
        {
            Path = 1,
            DropPod,
            Extractor,
            WorkCamp,
            Barrel,
            Wall,
        }







        static void Place(int x, int y, short data) 
        {
            sector[x, y].ID = data == 4 ?
                (short)Base_Building_Game.General.TileID.Wood : 
                (short)Base_Building_Game.General.TileID.Grass;



            Base_Building_Game.General.hotbar.BuildBuilding(data switch
            {
                (short)BuildingEnum.Path => BuildingID.Path,
                (short)BuildingEnum.DropPod => BuildingID.DropPod,
                (short)BuildingEnum.Extractor => BuildingID.Extractor,
                (short)BuildingEnum.WorkCamp => BuildingID.WorkCamp,
                (short)BuildingEnum.Barrel => BuildingID.Barrel,
                (short)BuildingEnum.Wall => BuildingID.Wall,

                _ => BuildingID.None,
            }, x, y);
        }



        internal static readonly short[] validTiles = new short[]
        {
            (short)Base_Building_Game.General.TileID.Grass,
            (short)Base_Building_Game.General.TileID.Stone,
            (short)Base_Building_Game.General.TileID.Iron,
            (short)Base_Building_Game.General.TileID.Diamond,
            (short)Base_Building_Game.General.TileID.Wood,
        };

        static bool Buildable(int x, int y)
        {
            if (0 > x || 0 > y) { return false; }
            int sectorSize = Base_Building_Game.General.SectorSize;
            if (x >= sectorSize || y >= sectorSize) { return false; }

            return (
                validTiles.Contains(sector[x, y].ID)) &&
                ((sector[x, y].building is null) ||
                (sector[x, y].building.ID == (short)Base_Building_Game.General.BuildingID.Wall) ||
                (sector[x, y].building.ID == (short)Base_Building_Game.General.BuildingID.None));
        }
    }
}
