using Short_Tools;
using static Short_Tools.General;
using Debugger = Short_Tools.ShortDebugger;
using SDL2;
using static SDL2.SDL;

namespace Base_Building_Game
{
    public static partial class General
    {
        public static void Main()
        {
            LoadSettings();
            LoadImages();

            renderer.Start();

            while (Running)
            {
                handler.HandleInputs(ref Running);

                Thread.Sleep(50);
            }

            Cleanup();
        }
    }
}