using Short_Tools;
using static Short_Tools.General;
using Debugger = Short_Tools.ShortDebugger;
using SDL2;
using static SDL2.SDL;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Reflection.Metadata;



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
            ShowConsole();
            debugger.ChangeDisplayPriority(Debugger.Priority.DEBUG);
#else
            HideConsole();
            debugger.ChangeDisplayPriority(Debugger.Priority.ERROR);
#endif
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);

            Initialise();



            

          


            SetSeed(); // <- stick a seed in here to generate the same world :) (you can get the seed via the RandSeed var)
            
            
            
            //if (!File.Exists("Saves\\Test.SWrld"))
            //{
            //    CreateWorld();
            //    SaveWorld(world, "Test.SWrld");
            //}
            //else
            //{
            //    LoadWorld($"./Saves/Test.SWrld");
            //}

            #region Random Testing Stuff
            hotbar.SetBuilding(BuildingID.Wall);
            hotbar.SetBuilding(BuildingID.Bridge);
            hotbar.SetBuilding(BuildingID.Extractor);
            hotbar.SetBuilding(BuildingID.DropPod);
            hotbar.SetBuilding(BuildingID.SmallPort);
            hotbar.SetBuilding(BuildingID.MedPort);
            hotbar.SetBuilding(BuildingID.LargePort);
            hotbar.SetBuilding(BuildingID.WorkCamp);
            hotbar.SetBuilding(BuildingID.Barrel);
            hotbar.SetBuilding(BuildingID.Pipe);

            //hotbar.BuildBuilding(BuildingID.DropPod, player.blockX, player.blockY);

            BoatResearch[(short)BoatID.Destroyer] = 2;


            //ReqSaveMapImage("uhhh.png");

            //EnemyUnit test = new EnemyUnit();
            //test.pos = player.pos;

            #endregion Random Stuff




            //new DropInAni();
          

            //PlayCutscene("IntroCutscene");


            while (Running)
            {
                renderer.CheckSDLErrors();


                if (WaitingForWorldCreate)
                {
                    CreateWorld();
                }
                if (WaitingForWorldSave)
                {
                    SaveWorld(tempWorldName);
                }



                #region In Menu
                if (MenuState != MenuStates.InGame)
                {
                    Thread.Sleep(50);
                    dt = GetDt(ref LFT);
                    continue;
                }
                #endregion

                #region In Game
                if (renderer.ActiveCutscene is null)
                {
                    //if (renderer.images["Map"] == IntPtr.Zero)
                    //{
                    //    //TODO: change this to be world.name
                    //    ReqSaveMapImage(renderer.worldName + ".png");
                    //    renderer.images["Map"] = renderer.LoadImage(renderer.worldName + ".png");
                    //}

                    player.Move((int)dt);

                    Tick((int)dt);

                    Thread.Sleep(10);

                    dt = GetDt(ref LFT);
                    
                    RunActiveEntities((int)dt);
                }
                #endregion In Game

                #region In Cutscene
                else
                {
                    Thread.Sleep(100);
                    dt = GetDt(ref LFT);
                }
                #endregion In Cutscene
            }

            //SaveWorld(world, "Test.SWrld");


            Cleanup();

        }














        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            // Put your own handler here

            switch (ctrlType)
            {
                case CtrlTypes.CTRL_CLOSE_EVENT:

                    Running = false;
                    Cleanup();

                    break;
            }

            return true;
        }





        #region unmanaged
        // Declare the SetConsoleCtrlHandler function
        // as external and receiving a delegate.

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        // A delegate type to be used as the handler routine
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // An enumerated type for the control messages
        // sent to the handler routine.

        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }




        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        static IntPtr ConsoleHandler = GetConsoleWindow();

        // Hide
        static void HideConsole() => ShowWindow(ConsoleHandler, SW_HIDE);

        // Show
        static void ShowConsole() => ShowWindow(ConsoleHandler, SW_SHOW);

        #endregion

    }
}