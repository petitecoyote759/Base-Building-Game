using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;
using Prio = Short_Tools.ShortDebugger.Priority;



namespace Base_Building_Game
{
    public static partial class General
    {
        static string GetPath([CallerFilePath] string file = "") => file;
        static string GetPathOfGame()
        {
            string[] stuff = GetPath().Split('\\');
            string path = "";
            for (int i = 0; i < stuff.Length - 2; i++)
            {
                path += stuff[i] + "\\";
            }
            return path;
        }






        static void LoadSettings()
        {
            #if DEBUG
            settings.Debugging = true;
            #endif

            if (File.Exists("Settings.ini"))
            {
                InitialisePercent = 50;
                // if the executable is in the same area as settings
                settings.LoadINI<Settings>("Settings.ini");
            }
            else
            {
                string path = GetPathOfGame();

                InitialisePercent = 20;

                if (File.Exists(path + "Settings.ini"))
                {
                    InitialisePercent = 50;

                    // If the files are idk, like how the fings work
                    settings.LoadINI<Settings>(path + "Settings.ini");
                }
                else
                {
                    debugger.AddLog("Settings did not load properly");
                }
            }

            InitialisePercent = 100;
        }






        static void LoadImages()
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

            int i = 0;
            foreach (var pair in images) 
            {
                InitialisePercent = 50 * i / images.Count;

                if (!TexturePackedImages[pair.Key])
                {
                    images[pair.Key] = path + pair.Value;
                }
                i++;
            }

            renderer.Load_Images(images);
            InitialisePercent = 100;

            UpdateDictionaries();
        }



        static void LoadText()
        {
            string path;

            if (Directory.Exists(GetPathOfGame() + "Images\\Text"))
            {
                path = GetPathOfGame() + "Images\\Text\\";
            }
            else
            {
                path = "Images\\Text\\";
            }

            renderer.SetupText(path + "Aller_Bd.ttf");

            // C:\Users\Matt\OneDrive\The Game\Base Building Game\Text\Aller_Bd.ttf
        }





        public static void AddLog(string message, Prio priority = Prio.INFO)
        {
            debugger.AddLog(message, priority);
        }






        static void SetSeed(int seed)
        {
            randy = new Random(seed);
        }
        static void SetSeed() { }









        /// <summary>
        /// Runs on closing of the game, ensure that everything is saved and nice and tidy ;)
        /// </summary>
        static void Cleanup()
        {
            if (CleanedUp) 
            {
#if DEBUG
                Print("Accidental Cleanup recall", ConsoleColor.Cyan);
#endif
                return;
            }
            debugger.AddLog("Entering cleanup function", Prio.DEBUG);
            CleanedUp = true;


            debugger.CleanFiles(4);
            debugger.Save();

            renderer.debugger.Save();
            renderer.debugger.CleanFiles(5);


            renderer.Stop();

            Cutscene.Cleanup();


            foreach (var pair in FancyTiles)
            {
                foreach (IntPtr image in pair.Value)
                {
                    SDL2.SDL.SDL_DestroyTexture(image);
                }
            }

            foreach (var pair in InitialImages)
            {
                SDL2.SDL.SDL_DestroyTexture(pair.Value);
            }


            if (File.Exists("Settings.ini"))
            {
                settings.SaveINI<Settings>("Settings.ini");
            }
            else if (File.Exists(GetPathOfGame() + "Settings.ini"))
            {
                settings.SaveINI<Settings>(GetPathOfGame() + "Settings.ini");
            }
        }











        public static T[,] Make2DArray<T>(T[] input, int height, int width)
        {
            T[,] output = new T[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    output[i, j] = input[i * width + j];
                }
            }
            return output;
        }












        public static string ByteArrayToString(params byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }




        public static double ToRadians(this double Angle) => Angle * Math.PI / 180d;
        public static double ToDegrees(this double Angle) => Angle * 180f / Math.PI;

        public static float ToRadians(this float Angle) => Angle * MathF.PI / 180f;
        public static float ToDegrees(this float Angle) => Angle * 180f / MathF.PI;
    }
}
