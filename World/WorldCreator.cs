using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Base_Building_Game.General;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;

#pragma warning disable CS8602



namespace Base_Building_Game
{
    public static partial class General
    {
        const int SeedCount = 64;
        const int PushItterations = 8;
        const int DefForce = 100;



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
                            DefForce / ((Seed - OtherSeed).MagSquared() + 1);
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

            new IslandCreator(Seed, sector);

            sector.map[Seed.x, Seed.y] = new Tile(TileID.Diamond);
        }








        static bool ChanceFunction(IVect pos, IVect tested)
        {
            int MidDistance = (pos - new IVect(SectorSize / 2, SectorSize / 2)).Mag();
            int IslandDist = (pos - tested).Mag();
            float Expo = (IslandDist / 2) - (2.5f * (2 + (MidDistance / SectorSize)));

            return randy.Next(0, 10000) < 10000f / (1 + MathF.Pow(2, Expo) - 1);
        }






        class IslandCreator
        {
            List<IslandTile> NotFinished = new List<IslandTile>();
            List<IslandTile> Done = new List<IslandTile>();

            IVect pos;

            public IslandCreator(IVect pos, Sector sector)
            {
                NotFinished.Add(new IslandTile(pos));

                while (NotFinished.Count != 0)
                {
                    IslandTile[] tempTiles = NotFinished.ToArray();
                    foreach (IslandTile tile in tempTiles)
                    {
                        if (ChanceFunction(pos, tile.pos))
                        {
                            sector[pos.x, pos.y] = new Tile(TileID.Grass);
                        }
                        else
                        {
                            Done.Add(tile);
                            NotFinished.Remove(tile);
                            break;
                        }






                        if (sector[tile.pos.x, tile.pos.y - 1].ID == (short)TileID.Grass) { tile.directions[0] = false; }
                        if (sector[tile.pos.x, tile.pos.y + 1].ID == (short)TileID.Grass) { tile.directions[2] = false; }
                        if (sector[tile.pos.x - 1, tile.pos.y].ID == (short)TileID.Grass) { tile.directions[3] = false; }
                        if (sector[tile.pos.x + 1, tile.pos.y].ID == (short)TileID.Grass) { tile.directions[1] = false; }




                        if (tile.directions[0])
                        {
                            if (sector[tile.pos.x, tile.pos.y - 1].ID == (short)TileID.Ocean)
                            {
                                IslandTile tempTile = new IslandTile(tile.pos + new IVect(0, -1));
                                if (!(NotFinished.Any((ITile) => ITile == tempTile) || Done.Any((ITile) => ITile == tempTile)))
                                {
                                    NotFinished.Add(tempTile);
                                }
                            }
                        }
                        if (tile.directions[1])
                        {
                            if (sector[tile.pos.x + 1, tile.pos.y].ID == (short)TileID.Ocean)
                            {
                                IslandTile tempTile = new IslandTile(tile.pos + new IVect(1, 0));
                                if (!(NotFinished.Any((ITile) => ITile == tempTile) || Done.Any((ITile) => ITile == tempTile)))
                                {
                                    NotFinished.Add(tempTile);
                                }
                            }
                        }
                        if (tile.directions[2])
                        {
                            if (sector[tile.pos.x, tile.pos.y + 1].ID == (short)TileID.Ocean)
                            {
                                IslandTile tempTile = new IslandTile(tile.pos + new IVect(0, 1));
                                if (!(NotFinished.Any((ITile) => ITile == tempTile) || Done.Any((ITile) => ITile == tempTile)))
                                {
                                    NotFinished.Add(tempTile);
                                }
                            }
                        }
                        if (tile.directions[3])
                        {
                            if (sector[tile.pos.x - 1, tile.pos.y].ID == (short)TileID.Ocean)
                            {
                                IslandTile tempTile = new IslandTile(tile.pos + new IVect(-1, 0));
                                if (!(NotFinished.Any((ITile) => ITile == tempTile) || Done.Any((ITile) => ITile == tempTile)))
                                {
                                    NotFinished.Add(tempTile);
                                }
                            }
                        }

                        Done.Add(tile);
                        NotFinished.Remove(tile);
                        sector[tile.pos.x, tile.pos.y] = new Tile(TileID.Grass);
                    }
                }





                //if (sector[pos.x, pos.y - 1].ID == (short)TileID.Grass) { temp.directions[0] = false; }
                //if (sector[pos.x, pos.y + 1].ID == (short)TileID.Grass) { temp.directions[2] = false; }
                //if (sector[pos.x - 1, pos.y].ID == (short)TileID.Grass) { temp.directions[3] = false; }
                //if (sector[pos.x + 1, pos.y].ID == (short)TileID.Grass) { temp.directions[1] = false; }



                //foreach (IslandTile tile in Done)
                //{
                //
                //}
            }
        }



        class IslandTile
        {
            public bool[] directions = new bool[4] { true, true, true, true };
            public IVect pos;

            public IslandTile(IVect pos)
            {
                this.pos = pos;
            }

            public static bool operator ==(IslandTile lhs, IslandTile rhs) { return lhs.pos == rhs.pos; }
            public static bool operator !=(IslandTile lhs, IslandTile rhs) { return lhs.pos != rhs.pos; }
        }
    }
}
