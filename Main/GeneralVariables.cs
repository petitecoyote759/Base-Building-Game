using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Debugger = Short_Tools.ShortDebugger;




namespace Base_Building_Game
{
    public static partial class General
    {
        static Random randy = new Random();

        static Renderer renderer = new Renderer();
        static Settings settings = new Settings();

#if DEBUG
        static Debugger debugger = new Debugger("General", "Logs\\", Debugger.Flags.DISPLAY_ON_ADD_LOG);
#else
        static Debugger debugger = new Debugger("General", "Logs\\");
#endif


        static Handler handler = new Handler();
        static Player player = new Player();
        static Hotbar hotbar = new Hotbar();

        static World world;

        static Sector ActiveSector = new Sector(false);



        static List<FBuilding> FBuildings = new List<FBuilding>();


        /// <summary>
        /// Bool representing if the player is currently in a game (online or offline)
        /// </summary>
        static bool InGame = false;

        /// <summary>
        /// State of the game, if turned to false, the main function loop will stop, and so will the program
        /// </summary>
        static bool Running = true;





        const int SectorSize = 2048;
    }
}
