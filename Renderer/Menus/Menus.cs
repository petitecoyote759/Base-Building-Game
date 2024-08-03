using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Short_Tools.General;
using Short_Tools;







namespace Base_Building_Game
{
    public enum MenuStates
    {
        StartScreen,

        SelectOnlineOffline,

        OfflineCreateLoadWorld,


        /// <summary>
        /// Offline menu for creating a world, should have an option to put in a seed, world size, and uhhh idk
        /// difficulty too? and cheats + structures, make it so it is extendable with click to change
        /// </summary>
        OfflineCreateWorld,


        /// <summary>
        /// Offline menu for loading a world, should pop up with all of the worlds in your folder, scroll bar.
        /// when you press on a world, show some stats about it (playtime, version, time last played)
        /// </summary>
        OfflineLoadWorld,




        InGame
    }
}
