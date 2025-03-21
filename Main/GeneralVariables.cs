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
        internal static int RandSeed = randysParent.Next();
        public static Random randy = new Random(RandSeed);

        public static Renderer renderer = new Renderer();
        static Settings settings = new Settings();

#if DEBUG
        public static Debugger debugger = new Debugger("General", "Logs\\", Debugger.Flags.DISPLAY_ON_ADD_LOG);
#else
        public static Debugger debugger = new Debugger("General", "Logs\\");
#endif


        public static Handler handler = new Handler();
        public static Player player = new Player();
        public static Hotbar hotbar = new Hotbar();

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
        /// Time of day in milliseconds
        /// </summary>
        static int Time = 0;


        /// <summary>
        /// Time it takes for a full day night cycle in milliseconds
        /// </summary>
        const int TimePerDay = 100000; // 100000 default



        /// <summary>
        /// Enum variable to show what state the menu is in.
        /// </summary>
        public static MenuStates MenuState = MenuStates.StartScreen;


        /// <summary>
        /// bool representing if the cleanup function has already been called
        /// </summary>
        static bool CleanedUp = false;



        /// <summary>
        /// bool representing if the game is in the very first start up menu, which contains the loading of the game images and 
        /// basic functionalities for the menu
        /// </summary>
        public static bool Initialising = true;

        public static bool InitImagesLoaded = false;

        public static bool SavingMapImage = false;




        /// <summary>
        /// Distance away from the player where they stop beziering
        /// </summary>
        public const int MenBezierDistance = 50;//200;
        /// <summary>
        /// Distance away from player where they teleport
        /// </summary>
        public const int MenTeleportDistance = 150;

        public const float PathSpeedMultiplier = 1.5f;






        public const int SectorSize = 2048;
    }
}
