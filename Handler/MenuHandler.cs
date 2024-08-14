using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;


namespace Base_Building_Game
{
    public static partial class General
    {
        #region Create world vars
        static bool? SelectedOption = null; // null means no option, true means name and false means seed
        #endregion

        #region Load World Vars
        static MenuWorld? selectedWorld = null;
        #endregion




        //static int screenwidth { get => renderer.screenwidth; }
        //static int screenheight { get => renderer.screenheight; }


        public static void HandleMenus(string inp)
        {
            int screenwidth = renderer.screenwidth;
            int screenheight = renderer.screenheight;

            switch (MenuState)
            {
                case MenuStates.StartScreen:

                    if (inp == "SPACE")
                    {
                        MenuState = MenuStates.SelectOnlineOffline;
                    }

                    break;


                case MenuStates.SelectOnlineOffline:

                    switch (inp)
                    {
                        case "Escape":
                            MenuState = MenuStates.StartScreen;
                            break;

                        case "Mouse":

                            IVect mpos = getMousePos();

                            // correct y level for the buttons
                            if (screenheight * 4 / 10 <= mpos.y && screenheight * 6 / 10 >= mpos.y)
                            {
                                if (screenwidth * 1 / 10 <= mpos.x && screenwidth * 4 / 10 >= mpos.x)
                                {
                                    MenuState = MenuStates.OnlineFindCreate;
                                }
                                else if (screenwidth * 6 / 10 <= mpos.x && screenwidth * 9 / 10 >= mpos.x)
                                {
                                    MenuState = MenuStates.OfflineCreateLoadWorld;
                                }
                            }


                            break;
                    }

                    break;


                case MenuStates.OnlineFindCreate:

                    if (inp == "ESCAPE") 
                    {
                        MenuState = MenuStates.StartScreen;
                    }
                    break;


                case MenuStates.OfflineCreateLoadWorld:

                    if (inp == "ESCAPE")
                    {
                        MenuState = MenuStates.StartScreen;
                    }
                    if (inp == "Mouse")
                    {
                        IVect mpos = getMousePos();

                        if (0 <= mpos.y && mpos.y <= screenheight / 15) // same y level as buttons
                        {
                            if (screenwidth / 10 <= mpos.x && mpos.x <= screenwidth * 4 / 10)
                            {
                                MenuState = MenuStates.OfflineCreateWorld;
                            }
                            if (screenwidth * 6 / 10 <= mpos.x && mpos.x <= screenwidth * 9 / 10)
                            {
                                string[] worlds = Directory.GetDirectories("Saves");

                                renderer.loadableWorlds = new MenuWorld[worlds.Length];
                                for (int i = 0; i < worlds.Length; i++)
                                {
                                    renderer.loadableWorlds[i] = new MenuWorld(worlds[i]);
                                }

                                renderer.loadableWorlds = (
                                    from world in renderer.loadableWorlds
                                    orderby File.GetLastAccessTimeUtc($"Saves\\{world.name}\\{world.name}.SWrld") descending
                                    select world
                                    ).ToArray();

                                MenuState = MenuStates.OfflineLoadWorld;

                            }
                        }
                    }
                    break;



                case MenuStates.OfflineCreateWorld:

                    if (inp == "ESCAPE") 
                    {
                        if (SelectedOption is not null) { SelectedOption = null; }
                        else
                        {
                            MenuState = MenuStates.OfflineCreateLoadWorld;
                            break;
                        }
                    }
                    else if (inp == "Mouse")
                    {
                        IVect mpos = getMousePos();


                        if (screenheight * 3 / 10 <= mpos.y && mpos.y <= screenheight * 5 / 10)
                        {
                            if (screenwidth * 1 / 5 <= mpos.x && mpos.x <= screenwidth * 2 / 5)
                            {
                                SelectedOption = true;
                            }
                            else if (screenwidth * 3 / 5 <= mpos.x && mpos.x <= screenwidth * 4 / 5)
                            {
                                SelectedOption = false;
                            }
                        }

                        if (screenheight * 8 / 10 <= mpos.y && mpos.y <= screenheight * 9 / 10)
                        {
                            if (screenwidth / 2 - screenwidth / 8 <= mpos.x && mpos.x <= screenwidth / 2 - screenwidth / 8 + screenwidth / 4)
                            {
                                if (renderer.seed != "")
                                {
                                    if (int.TryParse(renderer.seed, out int seed)) { SetSeed(seed); }
                                }
                                if (renderer.worldName != "")
                                {
                                    //TODO: make settings affect this
                                    //TODO: make load screen for this -> move it to main thread

                                    ReqCreateWorld();
                                    ReqSaveWorld(renderer.worldName);
                                }
                            }
                        }
                    }
                    #region Typing
                    else if (inp.Length == 1) //TODO: make this work with _!-()[]{}'@~#:;><?
                    {
                        if (ActiveKeys["LSHIFT"]) { inp = inp.ToUpper(); }
                        if (SelectedOption is bool option)
                        {
                            if (option == true) // name
                            {
                                if (renderer.worldName.Length < renderer.MaxSeedAndNameLength)
                                {
                                    renderer.worldName += inp;
                                }
                            }
                            if (option == false)
                            {
                                if (renderer.seed.Length < renderer.MaxSeedAndNameLength)
                                {
                                    renderer.seed += inp;
                                }
                            }
                        }
                    }
                    else if (inp == "SPACE")
                    {
                        if (SelectedOption is bool option)
                        {
                            if (option == true) // name
                            {
                                if (renderer.worldName.Length < renderer.MaxSeedAndNameLength)
                                {
                                    renderer.worldName += " ";
                                }
                            }
                            if (option == false)
                            {
                                if (renderer.seed.Length < renderer.MaxSeedAndNameLength)
                                {
                                    renderer.seed += " ";
                                }
                            }
                        }
                    }
                    else if (inp == "BACKSPACE")
                    {
                        if (SelectedOption is bool option)
                        {
                            if (option == true) // name 
                            {
                                if (renderer.worldName.Length > 0)
                                {
                                    renderer.worldName = renderer.worldName.Substring(0, renderer.worldName.Length - 1);
                                }
                            }
                            if (option == false)
                            {
                                if (renderer.seed.Length > 0)
                                {
                                    renderer.seed = renderer.seed.Substring(0, renderer.seed.Length - 1);
                                }
                            }
                        }
                    }
                    #endregion Typing

                    break;



                case MenuStates.OfflineLoadWorld:

                    if (inp == "ESCAPE")
                    {
                        MenuState = MenuStates.OfflineCreateLoadWorld;
                        foreach (MenuWorld world in renderer.loadableWorlds)
                        {
                            world.Dispose();
                        }
                        renderer.loadableWorlds = Array.Empty<MenuWorld>();
                        selectedWorld = null;
                    }


                    if (inp == "Mouse")
                    {
                        IVect mpos = getMousePos();

                        if (screenwidth * 35 / 200 <= mpos.x && mpos.x <= screenwidth * 98 / 200)
                        {
                            for (int i = 0; i < renderer.loadableWorlds.Length; i++)
                            {
                                int height = screenheight * 10 / 40 + (i * ((screenheight * 12 / 20) / Renderer.worldsPerPage));

                                if (height <= mpos.y && mpos.y <= height + (screenheight * 12 / 20) / Renderer.worldsPerPage)
                                {
                                    selectedWorld = renderer.loadableWorlds[i];
                                }
                            }
                        }


                        if (selectedWorld is not null)
                        {
                            if (screenwidth * 25 / 48 <= mpos.x && mpos.x <= screenwidth * 37 / 48)
                            {
                                if (screenheight * 15 / 20 <= mpos.y && mpos.y <= screenheight * 17 / 20) // loading world
                                {
                                    ReqLoadWorld(selectedWorld.path + "\\" + selectedWorld.name + ".SWrld");
                                    renderer.images["Map"] = renderer.L(selectedWorld.path + "\\" + selectedWorld.name + ".png");
                                }
                            }
                        }
                    }





                    break;










                case MenuStates.Loading:
                    break;
                case MenuStates.InGame:
                    break;





                default:
                    if (inp == "ESCAPE")
                    {
                        MenuState = MenuStates.StartScreen;
                    }
                    break;
            }
        }
    }
}
