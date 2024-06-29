using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Base_Building_Game.General;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;


namespace Base_Building_Game
{
    public static partial class General
    {
        const int SeedCount = 64;
        const int PushItterations = 8;
        const int DefForce = 1_000;



        public static void CreateWorld()
        {
            world = new World();

            int size = World.size;

            world.sectors[(size + 1) / 2, (size + 1) / 2] = CreatePFSector();

            ActiveSector = world[(World.size + 1) / 2, (World.size + 1) / 2];

            player.pos = new IVect(SectorSize / 2, SectorSize / 2);
            player.camPos = new IVect(SectorSize / 2, SectorSize / 2);
        }



        public static Sector CreatePFSector() // see https://www.desmos.com/calculator/zgaxgsk2ws for more info
        {
            Sector sector = new Sector();

            sector.map = Make2DArray(Enumerable.Repeat(new Tile(TileID.Ocean), SectorSize * SectorSize).ToArray(), SectorSize, SectorSize);


            List<IVect> Seeds = new List<IVect>();

            for (int index = 0; index < SeedCount; index++)
            {
                IVect Seed = new IVect(
                    GetCentralisedValue(),
                    GetCentralisedValue()
                    );



                if (Seeds.Count == 0)
                {
                    Seed = new IVect(SectorSize / 2, SectorSize / 2);

                    Seeds.Add(Seed);

                    GrowIslandSeed(Seed, sector, true);
                    continue;
                }




                for (int i = 0; i < PushItterations; i++)
                {


                    IVect ResultantForce = new IVect();
                    foreach (IVect OtherSeed in Seeds)
                    {
                        ResultantForce += (Seed - OtherSeed) * DefForce * 
                            Math.Min(DefForce / ((Seed - OtherSeed).MagSquared() + 1), 100 * DefForce);
                        // other islands sum

                        ResultantForce += new IVect(GetXForce(Seed.x), GetYForce(Seed.y));
                    }

                    Seed.x = Math.Min(Math.Max(Seed.x + ResultantForce.x, SectorSize / 10), SectorSize * 9 / 10);
                    Seed.y = Math.Min(Math.Max(Seed.y + ResultantForce.y, SectorSize / 10), SectorSize * 9 / 10);
                }

                Seeds.Add(Seed);

                GrowIslandSeed(Seed, sector);
            }

            StringBuilder SeedPos = new StringBuilder();
            foreach (IVect DSeed in Seeds) { SeedPos.Append(DSeed.ToString() + ", "); }
            debugger.AddLog(SeedPos.ToString(), Short_Tools.ShortDebugger.Priority.DEBUG);


            return sector;
        }


        static int GetCentralisedValue()
        {
            return (int)((Math.Pow(2d * randy.NextDouble() - 1d, 3) + 1d) * SectorSize / 2);
        }

        static int GetXForce(int x)
        {
            return 
                DefForce * SectorSize / ((x + 1) * 500) +
                DefForce * SectorSize / ((SectorSize - x + 1) * 500);
        }
        static int GetYForce(int y)
        {
            return
                DefForce * SectorSize / ((y + 1) * 500) +
                DefForce * SectorSize / ((SectorSize - y + 1) * 500);
        }




        static void GrowIslandSeed(IVect Seed, Sector sector, bool MainIsland = false)
        {
            if (sector.map is null) 
            { 
                debugger.AddLog("Map was null, idk how, cant recover", Short_Tools.ShortDebugger.Priority.CRITICAL);
                throw new Exception("What the freak");
            }

            sector.map[Seed.x, Seed.y] = new Tile(TileID.Grass);

            for (int x = Seed.x - 5; x < Seed.x + 5; x++)
            {
                for (int y = Seed.y - 5; y < Seed.y + 5; y++)
                {
                    sector.map[x, y] = new Tile(TileID.Grass);
                }
            }
        }
    }
}
