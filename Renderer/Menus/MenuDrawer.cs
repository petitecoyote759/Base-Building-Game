using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using static SDL2.SDL;
using Short_Tools;
using static Short_Tools.General;
using System.Runtime.CompilerServices;






namespace Base_Building_Game
{
    public static partial class General
    {
        public partial class Renderer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void DrawMenu()
            {
                switch (MenuState)
                {
                    case MenuStates.StartScreen:

                        Draw(0, 0, screenwidth, screenheight, "Background");

                        byte rgb = 0;

                        unchecked
                        {
                            rgb = (byte)(255 * (Math.Sin(DateTimeOffset.Now.ToUnixTimeMilliseconds() / 500d) + 2) / 4);
                        }

                        Write(
                            (screenwidth - (45 * 20)) / 2, // length of "Press SP...
                            screenheight * 6 / 10, 
                            60, 
                            60, 
                            "Press SPACE to start", 
                            new SDL_Color() { a = 255, r = rgb, b = rgb, g = rgb });

                        break; // Switch case for the start screen, pulsing space
                               // to start and alla that






                    case MenuStates.SelectOnlineOffline:

                        Draw(0, 0, screenwidth, screenheight, "Background");

                        DrawButton(
                            screenwidth * 1 / 10,
                            screenheight * 4 / 10,
                            screenwidth * 3 / 10,
                            screenheight * 2 / 10,
                            "Online");

                        DrawButton(
                            screenwidth * 6 / 10,
                            screenheight * 4 / 10,
                            screenwidth * 3 / 10,
                            screenheight * 2 / 10,
                            "Offline");


                        break;
                } 
            }
        }
    }
}
