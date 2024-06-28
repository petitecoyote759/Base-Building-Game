using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Short_Tools.General;



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
                // if the executable is in the same area as settings
                settings.LoadINI<Settings>("Settings.ini");
            }
            else
            {
                string path = GetPathOfGame();

                if (File.Exists(path + "Settings.ini"))
                {
                    // If the files are idk, like how the fings work
                    settings.LoadINI<Settings>(path + "Settings.ini");
                }
                else
                {
                    debugger.AddLog("Settings did not load properly");
                }
            }
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


            foreach (var pair in images) { images[pair.Key] = path + pair.Value; }

            renderer.Load_Images(images);

            UpdateDictionaries();
        }





















        /// <summary>
        /// Runs on closing of the game, ensure that everything is saved and nice and tidy ;)
        /// </summary>
        static void Cleanup()
        {
            debugger.CleanFiles(5);
            renderer.Stop();


            if (File.Exists("Settings.ini"))
            {
                settings.SaveINI<Settings>("Settings.ini");
            }
            else if (File.Exists(GetPathOfGame() + "Settings.ini"))
            {
                settings.SaveINI<Settings>(GetPathOfGame() + "Settings.ini");
            }
        }











        private static T[,] Make2DArray<T>(T[] input, int height, int width)
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
    }
}
