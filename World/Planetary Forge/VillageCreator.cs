using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;
using Sector = Base_Building_Game.General.Sector;

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

        private static void Run(IVect seed, Sector inSector)
        {
            sector = inSector;

            Building[] buildings = new Building[] {

            new Building(new short[,] {
            { 0, 0, 0, 1, 0, 0, 0 },
            { 0, 1, 1, 1, 1, 1, 0 },
            { 0, 1, 0, 0, 0, 1, 0 },
            { 1, 1, 0, 0, 0, 1, 1 },
            { 0, 1, 0, 0, 0, 1, 0 },
            { 0, 1, 1, 1, 1, 1, 0 },
            { 0, 0, 0, 1, 0, 0, 0 },
            }, 0),

            new Building(new short[,] {
            { 0, 1, 0 },
            { 1, 1, 1 },
            { 0, 1, 0 }
            }, 3),

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
                nodesToVisit.Enqueue(new AccessPoint() { pos = point.pos + mapMiddle, dir = point.dir });
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














        static void Place(int x, int y, short data) 
        {
            if (data != 1) { return; }

            sector[x, y].building = new Base_Building_Game.General.Path(new IVect(x, y));
        }


        static bool Buildable(int x, int y)
        {
            if (0 > x || 0 > y) { return false; }
            int sectorSize = Base_Building_Game.General.SectorSize;
            if (x >= sectorSize || y >= sectorSize) { return false; }

#warning DO this, need to see if it is grass and no building.
        }
    }
}
