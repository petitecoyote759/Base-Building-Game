using Short_Tools;
using static Short_Tools.General;
using Debugger = Short_Tools.ShortDebugger;
using SDL2;
using static SDL2.SDL;

namespace Base_Building_Game
{
    public static partial class General
    {
        static void Main()
        {
            LoadSettings();
            debugger.AddLog("not Started");
            LoadImages();
            debugger.AddLog("quarter Started");

            World.CreateWorld();
            debugger.AddLog("half Started");
            renderer.Start();
            debugger.AddLog("Started");

            while (Running)
            {
                handler.HandleInputs(ref Running);

                Thread.Sleep(50);
            }

            Cleanup();
        }
    }
}