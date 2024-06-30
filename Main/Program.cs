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

            CreateWorld();

            renderer.Start();
            SaveWorld(world, "test");
            LoadWorld($"./Saves/test");
            while (Running)
            {
                handler.HandleInputs(ref Running); 
                player.Move((int)dt);

                Thread.Sleep(10);

                dt = GetDt(ref LFT);
            }

            

            Cleanup();
        }
    }
}