using Short_Tools;
using static Short_Tools.General;
using Debugger = Short_Tools.ShortDebugger;
using SDL2;
using static SDL2.SDL;

namespace Base_Building_Game
{
    public static partial class General
    {
        static long dt = 0;
        static long LFT = DateTimeOffset.Now.ToUnixTimeMilliseconds(); // last frame time

        //Bayley was here
        static void Main()
        {
#if DEBUG
            debugger.ChangeDisplayPriority(Debugger.Priority.DEBUG);
#else
            debugger.ChangeDisplayPriority(Debugger.Priority.ERROR);
#endif

            LoadSettings();

            LoadImages();
            LoadFancyTiles();
            LoadText();



            if (!File.Exists("Saves\\Test.SWrld"))
            {
                CreateWorld();
                SaveWorld(world, "Test.SWrld");
            }
            else
            {
                LoadWorld($"./Saves/Test.SWrld");
            }

            hotbar.SetBuilding(BuildingID.Wall);
            hotbar.SetBuilding(BuildingID.Bridge);
            hotbar.SetBuilding(BuildingID.Extractor);
            hotbar.SetBuilding(BuildingID.DropPod);
            hotbar.SetBuilding(BuildingID.SmallPort);
            hotbar.SetBuilding(BuildingID.MedPort);
            hotbar.SetBuilding(BuildingID.LargePort);
            hotbar.SetBuilding(BuildingID.WorkCamp);

            hotbar.BuildBuilding(BuildingID.DropPod, player.blockX, player.blockY);

            BoatResearch[(short)BoatID.Destroyer] = 2;
           

            renderer.Start();

            SaveMapImage("uhhh.png");


            while (Running)
            {
                if (SDL_GetError() != "")
                {
                    AddLog("SDL Error -> " + SDL_GetError(), Debugger.Priority.ERROR);
                    SDL_ClearError();
                }


                handler.HandleInputs(ref Running); 
                player.Move((int)dt);

                Tick((int)dt);

                Thread.Sleep(10);

                dt = GetDt(ref LFT);
                RunActiveEntities((int)dt);
            }

            SaveWorld(world, "Test.SWrld");


            Cleanup();
            
        }
    }
}