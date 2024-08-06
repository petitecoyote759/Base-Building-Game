using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        static string tempWorldName = "";
        static bool WaitingForWorldSave = true;

        public static void ReqSaveWorld(string worldname)
        {
            while (WaitingForWorldSave) { Thread.Sleep(5); }

            tempWorldName = worldname;
            WaitingForWorldSave = true;
        }









        public static void SaveWorld(string worldname) // maybe path?
        {
            WaitingForWorldSave = false;
            string ResearchString = "";

            SectorJson[,] SectorData = new SectorJson[World.size,World.size];
            int i = -1;
            foreach (Sector S in world.sectors)
            {
                i++;
                if (S.map is null)
                {
                    SectorData[i / World.size, i % World.size] = new SectorJson() // Empty Tile
                    {
                        MapData = "",
                        ExtraInfo = ""
                    };
                    continue;
                }

                StringBuilder ThisMapData = new StringBuilder(); // String builder is faster than adding strings, learnt that after 30 minutes of debugging
                string ThisExtraInfo = "";
                
                for (int x = 0; x < S.map.GetLength(0); x++)
                {
                    for (int y = 0; y < S.map.GetLength(1); y++)
                    {
                        ThisMapData.Append(ConvertTileToCharacter(S.map[x,y]));
                       
                    }
                }

                 SectorData[i / World.size, i % World.size] = new SectorJson() // Not an empty tile 👏
                {
                    MapData = ThisMapData.ToString(),
                    ExtraInfo = ThisExtraInfo
                };
            }

            WorldJson SaveData = new()
            {
                PlayerX = player.camPos.X,
                PlayerY = player.camPos.Y,
                SectorX = player.SectorPos.X,
                SectorY = player.SectorPos.Y,
                Name = worldname,
                Research = ResearchString,
                Sectors = SectorData
            };

            if (!Directory.Exists("./Saves")) Directory.CreateDirectory("./Saves"); // Make sure saving directory exists
            StreamWriter SW = File.CreateText($"./Saves/{worldname}"); // Create/open file with name of worldname
            SW.Write(JsonConvert.SerializeObject(SaveData)); // Write savedata as string into text file
            SW.Close(); // Clean up
            SW.Dispose();
        }
    }
}