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
        static Random randysParent = new Random();
        static int RandSeed = randysParent.Next();
        static Random randy = new Random(RandSeed);

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

#pragma warning disable CS8618
        static World world; // it gets created when you load in the game, no way its gonna be null
#pragma warning restore CS8618

        static Sector ActiveSector = new Sector(false);



        static List<FBuilding> FBuildings = new List<FBuilding>();


        /// <summary>
        /// Bool representing if the player is currently in a game (online or offline)
        /// </summary>
        static bool InGame = false;

        /// <summary>
        /// State of the game, if turned to false, the main function loop will stop, and so will the program
        /// </summary>
        public static bool Running = true;




        /// <summary>
        /// Enum variable to show what state the menu is in.
        /// </summary>
        public static MenuStates MenuState = MenuStates.StartScreen;







        const int SectorSize = 2048;
    }
}
