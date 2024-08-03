using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                    if (inp == "ESCAPE")
                    {
                        MenuState = MenuStates.StartScreen;
                    }

                    break;
            }
        }
    }
}
