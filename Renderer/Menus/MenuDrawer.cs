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
            #region Create World Vars
            bool InAdvancedSettings = false;
            public string seed = "";
            public string worldName = "";
            int difficulty = 0; public enum Difficulty { Easy, Medium, Hard, Impossible }
            int size = 0; public enum WorldSize { Small, Medium, Large }
            bool enemyBases = true;
            bool FogOfWar = true;
            bool FreePlay = false;
            public int MaxSeedAndNameLength = 32;
            #endregion






            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
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






                    case MenuStates.OfflineCreateLoadWorld:

                        Draw(0, 0, screenwidth, screenheight, "Background");

                        DrawButton(screenwidth / 10, 0, screenwidth * 3 / 10, screenheight / 15, "Create World");

                        DrawButton(screenwidth * 6 / 10, 0, screenwidth * 3 / 10, screenheight / 15, "Load World");

                        break;





                    case MenuStates.OfflineCreateWorld:

                        #region Top and Background
                        Draw(0, 0, screenwidth, screenheight, "Background");

                        DrawButton(screenwidth / 10, 0, screenwidth * 3 / 10, screenheight / 15, "Create World");

                        DrawButton(screenwidth * 6 / 10, 0, screenwidth * 3 / 10, screenheight / 15, "Load World");
                        #endregion


                        #region CreateWorldBackground
                        //TODO: make this image not silly
                        Draw(
                            screenwidth / 6,
                            screenheight * 2 / 10,
                            screenwidth * 4 / 6,
                            screenheight * 13 / 20,
                            "MenuButton");
                        #endregion


                        #region General Or Advanced Settings
                        DrawButton(
                            screenwidth * 1 / 6,
                            screenheight * 1 / 10,
                            screenwidth * 2 / 6,
                            screenheight / 10,
                            "General Settings");

                        DrawButton(
                            screenwidth * 3 / 6,
                            screenheight * 1 / 10,
                            screenwidth * 2 / 6,
                            screenheight / 10,
                            "Advanced Settings");
                        #endregion


                        #region World Name
                        WriteOnFullWidth
                            (
                            screenwidth * 1 / 5,
                            screenheight * 8 / 30,
                            screenwidth / 5,
                            screenheight / 25,
                            "World Name"
                            );
                        DrawButton(
                            screenwidth * 1 / 5,
                            screenheight * 3 / 10,
                            screenwidth / 5,
                            screenheight / 20,
                            worldName
                            );
                        #endregion

                        #region seed
                        WriteOnFullWidth
                            (
                            screenwidth * 3 / 5,
                            screenheight * 8 / 30,
                            screenwidth / 5,
                            screenheight / 25,
                            "Seed (Optional)"
                            );
                        DrawButton(
                            screenwidth * 3 / 5,
                            screenheight * 3 / 10,
                            screenwidth / 5,
                            screenheight / 20,
                            seed
                            );
                        #endregion





                        #region CreateButton
                        DrawButton(
                            (screenwidth / 2) - (screenwidth / 8),
                            screenheight * 8 / 10,
                            screenwidth / 4,
                            screenheight / 10,
                            "Create");
                        #endregion

                        break;







                    case MenuStates.OnlineFindCreate:

                        Draw(0, 0, screenwidth, screenheight, "Background");

                        DrawButton(0, 0, screenwidth, screenheight, "OnlineFindCreate");

                        break;
                } 
            }
        }
    }
}
