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

            hotbar.BuildBuilding(2, player.x / 32, player.y / 32);
           

            renderer.Start();

            while (Running)
            {
                handler.HandleInputs(ref Running); 
                player.Move((int)dt);

                Tick((int)dt);

                Thread.Sleep(10);

                dt = GetDt(ref LFT);
            }

            

            Cleanup();
        }
    }
}