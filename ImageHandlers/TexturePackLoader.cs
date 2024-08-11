using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Short_Tools;
using static Short_Tools.General;
using DePrio = Short_Tools.ShortDebugger.Priority;




namespace Base_Building_Game
{
    public static partial class General
    {
#pragma warning disable CS8618 // this variable is defined before it is used, dont worry init
        static Dictionary<string, bool> TexturePackedImages;
#pragma warning restore CS8618




        public static void LoadTexturePacks()
        {
            TexturePackedImages = new Dictionary<string, bool>();
            foreach (var pair in images)
            {
                TexturePackedImages.Add(pair.Key, false);
            }


            string path;

            if (Directory.Exists(GetPathOfGame() + "TexturePacks"))
            {
                path = GetPathOfGame() + "TexturePacks\\";
            }
            else if (Directory.Exists("TexturePacks"))
            {
                path = "TexturePacks\\";
            }
            else
            {
                debugger.AddLog("Could not find texture pack file.", DePrio.WARN);
                return;
            }
            InitialisePercent = 10;











            string[] packs = Directory.GetDirectories(path);

            for (int i = 0; i < packs.Length; i++)
            {
                string pack = packs[i];

                InitialisePercent = (90 * i / packs.Length) + 10;
                string PackName = pack.Split('\\').Last();

                string jsonPath = path + "\\" + PackName + "\\Info.json";

                if (!File.Exists(jsonPath))
                {
                    debugger.AddLog($"Texture Pack {PackName} did not have a Info.json file", DePrio.ERROR);
                    continue;
                }


                string? json = File.ReadAllText(jsonPath);


                if (json is null) 
                {
                    debugger.AddLog($"Texture Pack {PackName} did not have a valid Info.json file (null error on file read)", DePrio.ERROR);
                    continue;
                }


                PackInfoJson? Info = null;

                try
                {
                    Info = JsonConvert.DeserializeObject<PackInfoJson>(json);
                }
                catch (Exception e)
                {
                    debugger.AddLog($"Texture Pack {PackName} did not have a valid Info.json file, json error => {e.Message}", DePrio.ERROR);
                    continue;
                }


                if (Info is null)
                {
                    debugger.AddLog($"Texture Pack {PackName} did not have a valid Info.json file", DePrio.ERROR);
                    continue;
                }


                if (!Info.Active) { continue; }




                path += PackName + "\\Images\\";

                foreach (var pair in images)
                {
                    if (File.Exists(path + pair.Value))
                    {
                        images[pair.Key] = path + pair.Value;
                        TexturePackedImages[pair.Key] = true;
                    }
                }
            }
        }

















        public class PackInfoJson
        {
            public int Priority { get; set; }
            public bool Active { get; set; }
        }
    }
}
