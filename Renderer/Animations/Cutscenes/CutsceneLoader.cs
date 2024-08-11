using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;



namespace Base_Building_Game
{
    public static partial class General
    {
        public static Dictionary<string, Cutscene> Cutscenes = new Dictionary<string, Cutscene>();

        public static void LoadCutscenes()
        {
            LoadCutsceneImages();
            InitialisePercent = 50;
            LoadCutsceneFiles();
            InitialisePercent = 100;
        }




        public static void LoadCutsceneImages()
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


            foreach (var pair in CutsceneImagePaths) { CutsceneImagePaths[pair.Key] = path + pair.Value; }

            CutsceneImages = CutsceneImagePaths.Values.ToDictionary(
                k => k.Split('\\').Last().Split('.').First(), 
                p => renderer.LoadImage(p)); // hehe
        }





        public static void LoadCutsceneFiles()
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

            string[] Cutscenes = Directory.GetDirectories(path + "Cutscenes\\");

            foreach (string file in Cutscenes)
            {
                if (File.Exists(file + "\\" + file.Split('\\').Last() + ".ctscn"))
                {
                    General.Cutscenes.Add(
                        file.Split('\\').Last(), 
                        new Cutscene(File.ReadAllText(file + "\\" + file.Split('\\').Last() + ".ctscn"), file.Split('\\').Last()));
                }
                else { debugger.AddLog($"Cutscene {file + "\\" + file.Split('\\').Last() + ".ctscn"} could not be found", ShortDebugger.Priority.ERROR); }
            }
        }
    }
}
