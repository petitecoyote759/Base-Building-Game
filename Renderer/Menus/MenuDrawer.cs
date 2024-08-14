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

            #region Load World Vars
            float barPos = 0f;
            public MenuWorld[] loadableWorlds;
            public const int worldsPerPage = 5;
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





                    case MenuStates.OfflineLoadWorld:

                        #region Top and Background
                        Draw(0, 0, screenwidth, screenheight, "Background");

                        DrawButton(screenwidth / 10, 0, screenwidth * 3 / 10, screenheight / 15, "Create World");

                        DrawButton(screenwidth * 6 / 10, 0, screenwidth * 3 / 10, screenheight / 15, "Load World");
                        #endregion

                        #region LoadWorldBackground
                        //TODO: make this image not silly
                        Draw( // world select
                            screenwidth * 1 / 6,
                            screenheight * 2 / 10,
                            screenwidth * 2 / 6,
                            screenheight * 13 / 20,
                            "MenuButton");

                        Draw( // scroll bar
                            screenwidth / 2,
                            screenheight * 2 / 10,
                            screenwidth * 1 / 48,
                            screenheight * 13 / 20,
                            "MenuButton");

                        Draw( // world details
                            screenwidth * 25 / 48,
                            screenheight * 2 / 10,
                            screenwidth * 3 / 12,
                            screenheight * 13 / 20,
                            "MenuButton");


                        DrawButton( // load world button
                            screenwidth * 25 / 48,
                            screenheight * 15 / 20,
                            screenwidth * 3 / 12,
                            screenheight * 2 / 20, // 17 / 20
                            "Load World");



                        #endregion

                        #region Worlds And Scroll Bar
                        for (int i = 0; i < loadableWorlds.Length; i++)
                        {
                            DrawButton( // world select
                            screenwidth * 35 / 200,
                            screenheight * 10 / 40 + (i * ((screenheight * 12 / 20) / worldsPerPage)),
                            screenwidth * 63 / 200,
                            (screenheight * 12 / 20) / worldsPerPage,
                            loadableWorlds[i].name);

                            // 2 / 10
                            // 13 / 20
                        }

                        Draw( // scroll bar
                            screenwidth / 2,
                            (screenheight * 2 / 10),
                            screenwidth * 1 / 48,
                            (int)(screenheight * 13 / 20 / ((float)Math.Max(loadableWorlds.Length, worldsPerPage) / worldsPerPage)),
                            "Hotbar");
                        #endregion

                        #region World Info
                        WriteOnFullWidth(
                            screenwidth * 25 / 48,
                            screenheight * 5 / 20,
                            screenwidth * 3 / 12,
                            screenheight * 3 / 40,
                            selectedWorld is null ? "Select World" : selectedWorld.name);
                        #endregion

                        break;







                    case MenuStates.OnlineFindCreate:

                        Draw(0, 0, screenwidth, screenheight, "Background");

                        DrawButton(0, 0, screenwidth, screenheight, "OnlineFindCreate");

                        break;









                    case MenuStates.Loading:

                        RenderClear();

                        double sin = Math.Sin(DateTimeOffset.Now.ToUnixTimeMilliseconds() / 500d);

                        int width = 100 + (int)(20 * sin);

                        Draw(screenwidth - 20 - width, screenheight - 20 - width, width, width, "Loading Spinner", sin * 180d);

                        break;
                } 
            }
        }
    }
}
