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
        static bool WaitingForWorldSave = false;

        public static void ReqSaveWorld(string worldname)
        {
            while (WaitingForWorldSave) { Thread.Sleep(5); }

            debugger.AddLog($"World has been saved successfully");
            tempWorldName = worldname;
            WaitingForWorldSave = true;
        }



        internal enum ExtraInfoType : short
        {
            Inventory = 1,
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
                StringBuilder ThisExtraInfo = new StringBuilder();
                
                for (int x = 0; x < S.map.GetLength(0); x++)
                {
                    for (int y = 0; y < S.map.GetLength(1); y++)
                    {
                        ThisMapData.Append(ConvertTileToCharacter(S.map[x,y]));

                        if (S.map[x, y]?.building?.inventory is not null)
                        {
                            Inventory inventory = S.map[x, y].building.inventory;
                            string extraData =
                                $"{(char)((short)ExtraInfoType.Inventory)}" +
                                $"{(char)((short)x)}" + // 16 bits so 1 char
                                $"{(char)((short)y)}" + // 16 bits so 1 char
                                $"{inventory.ToDataString()}";
                            /*
                            type (16 bits)
                            x (16 bits)
                            y (16 bits)
                            inventory (16 bits per item)
                            */
                            ThisExtraInfo.Append(extraData);
                            debugger.AddLog($"Just saved some tile data, {extraData}, the actual inventory is {inventory.ToString()} and its come out as {Inventory.FromString(extraData.Substring(4))}");
                        }
                    }
                }

                SectorData[i / World.size, i % World.size] = new SectorJson() // Not an empty tile 👏
                {
                    MapData = ThisMapData.ToString(),
                    ExtraInfo = ThisExtraInfo.ToString()
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

            if (!Directory.Exists("./Saves/" + worldname)) { Directory.CreateDirectory("./Saves/" + worldname); } // Make sure saving directory exists

            File.WriteAllText($"./Saves/{worldname}/{worldname}.SWrld", JsonConvert.SerializeObject(SaveData));
        }
    }
}