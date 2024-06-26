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
        public static string GetPath([CallerFilePath] string file = "") => file;


        public static void LoadSettings()
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
                string[] stuff = GetPath().Split('\\');
                string path = "";
                for (int i = 0; i < stuff.Length - 2; i++)
                {
                    path += stuff[i] + "\\";
                }
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
    }
}
