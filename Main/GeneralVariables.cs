using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Debugger = Short_Tools.ShortDebugger;




namespace Base_Building_Game
{
    public static partial class General
    {
        static Renderer renderer = new Renderer();
        static Settings settings = new Settings();
        static Debugger debugger = new Debugger("General");
        static Handler handler = new Handler();
        static Player player = new Player();

        static World world = new World();

        static Sector ActiveSector;



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
