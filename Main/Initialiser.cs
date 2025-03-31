using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;







namespace Base_Building_Game
{
    public static partial class General
    {
        static Dictionary<string, string> InitImagePaths = new Dictionary<string, string>()
        {
            { "SSLogo", "InitImages\\ShortLogo.png" },
            { "ProgressBar", "InitImages\\ProgressBar.png" },
            { "ProgressInSection", "InitImages\\ProgressInSection.png" }
        };
        static Dictionary<string, IntPtr> InitialImages = new Dictionary<string, IntPtr>();



        static int InitialisePercent = 0;
        static int InitFunctionsProgress = 0;


        static Dictionary<string, LoaderFunction> InitFunctions = new Dictionary<string, LoaderFunction> // Add init functions here <---
        {
            { "Loading Settings", new LoaderFunction(LoadSettings) }, 
            { "Loading Texture Packs", new LoaderFunction(LoadTexturePacks) },
            { "Loading Images", new LoaderFunction(LoadImages) },
            { "Loading Tile Sprite Sheets", new LoaderFunction(LoadFancyTiles) },
            { "Loading Cutscenes", new LoaderFunction(LoadCutscenes) },
            { "Loading Village Templates", new LoaderFunction(WorldGen.VillageGen.General.LoadTemplates) },
            { "Starting Entity Thread", new LoaderFunction(EntityThreadLoader) },
        };

        public static string CurrentLoaderFunction = "";


        public static void Initialise()
        {
            LoadText();

            renderer.Start();

            LoadInitialImages();

            InitImagesLoaded = true;


            debugger.AddLog("Starting LoaderFunctions", ShortDebugger.Priority.DEBUG);

            foreach (var FunctionPair in InitFunctions)
            {
                InitFunctionsProgress++;
                InitialisePercent = 0;
                CurrentLoaderFunction = FunctionPair.Key;
                FunctionPair.Value.LoadFunction();
            }

            debugger.AddLog("Completed LoaderFunctions", ShortDebugger.Priority.DEBUG);

            lock (renderer)
            {
                renderer.Enlarge();
            }
            Initialising = false;
        }











        public static void LoadInitialImages()
        {
            string path;

            if (Directory.Exists(GetPathOfGame() + "Images"))
            {
                path = GetPathOfGame() + "Images\\";
            }
            else
            {
                path = "Images\\";
            }


            foreach (var pair in InitImagePaths)
            {
                InitialImages.Add(pair.Key, renderer.LoadImage(path + pair.Value));
            }
        }








        public class LoaderFunction
        {
            public Action LoadFunction;

            public LoaderFunction(Action loadFunction)
            {
                LoadFunction = loadFunction;
            }

            public void Run() => LoadFunction();
        }
    }
}
