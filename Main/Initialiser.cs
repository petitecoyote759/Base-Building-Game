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
            { "SSLogo", "InitImages\\ShortLogo.png" }
        };
        static Dictionary<string, IntPtr> InitialImages = new Dictionary<string, IntPtr>();



        static int InitialisePercent = 0;


        static LoaderFunction[] InitFunctions = new LoaderFunction[]
        {
            new LoaderFunction(LoadSettings),
            new LoaderFunction(LoadTexturePacks),
            new LoaderFunction(LoadImages),
            new LoaderFunction(LoadFancyTiles),
            new LoaderFunction(LoadCutscenes),
        };



        public static void Initialise()
        {
            LoadText();
            LoadInitialImages();

            InitImagesLoaded = true;

            renderer.Start();

            debugger.AddLog("Starting LoaderFunctions", ShortDebugger.Priority.DEBUG);

            foreach (LoaderFunction function in InitFunctions)
            {
                function.LoadFunction();
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
                InitialImages.Add(pair.Key, renderer.L(path + pair.Value));
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
