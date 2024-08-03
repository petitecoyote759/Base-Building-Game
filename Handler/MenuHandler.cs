using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;


namespace Base_Building_Game
{
    public static partial class General
    {
        public static void HandleMenus(string inp)
        {
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

                            int screenwidth = renderer.screenwidth;
                            int screenheight = renderer.screenheight;
                            

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
                    break;
            }
        }
    }
}
