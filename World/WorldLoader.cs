using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Base_Building_Game.General;
using Priority = Short_Tools.ShortDebugger.Priority;


namespace Base_Building_Game
{
    public static partial class General
    {
        static string tempWorldPath = "";
        static bool WaitingForWorldLoad = false;

        public static void ReqLoadWorld(string path)
        {
            while (WaitingForWorldLoad) { Thread.Sleep(5); }

            tempWorldPath = path;
            WaitingForWorldLoad = true;
        }




        /// <summary>
        /// Loads the world, and active sector, from the specified JSON.
        /// </summary>
        /// <param name="path">The location of the JSON file containing the world data.</param>
        public static void LoadWorld(string path)
        {
            WaitingForWorldLoad = false;
            MenuState = MenuStates.Loading;

            //Loads the world from the json file.
            StreamReader reader = new StreamReader(path);
            string jsonString = reader.ReadToEnd();
            reader.Close();
            WorldJson? worldJson = JsonConvert.DeserializeObject<WorldJson>(jsonString);
            if (worldJson is null)
            {
                //Uh oh! Kind of an issue.
                debugger.AddLog("World JSON returned null", Priority.CRITICAL);

                //This should return to menu. Add this later.
                return;
            }

            //Setting the position of the player.
            player.x = worldJson.PlayerX;
            player.y = worldJson.PlayerY;
            


            //Initializing the world.
            world = new World();
            AddLog("World initialised", Priority.DEBUG);

            //Getting the position of the active sector from the JSON.
            int activeSectorX = worldJson.SectorX;
            int activeSectorY = worldJson.SectorY;

            //Creating the sector we are going to load, and pulling the data from the JSON.
            Sector loadedSector = new Sector(true);
            AddLog("Created the active sector", Priority.DEBUG);
            SectorJson sectorData = worldJson.Sectors[activeSectorX, activeSectorY];

            //This will fill up the sector with the tile data from the bit structure.
            for (int i = 0; i < SectorSize * SectorSize; i++)
            {
                short buildingID;
                Tile tile = ConvertCharacterToTile(sectorData.MapData[i]);
                buildingID = tile.building is null ? (short)BuildingID.None : tile.building.ID;
                loadedSector[i / SectorSize, i % SectorSize] = tile;
               
                try
                {
                    BuildingID Temp = (BuildingID)buildingID;
                }
                catch
                { AddLog($"Building ID {buildingID} was not contained in the enum!", Priority.ERROR); }
            }
            AddLog("Loaded tiles in active sector", Priority.DEBUG);

            world.sectors[activeSectorX, activeSectorY] = loadedSector;
            ActiveSector = loadedSector;

            for (int i = 0; i < SectorSize * SectorSize; i++)
            {
                BuildingID buildingID;
                ConvertCharacterToTile(sectorData.MapData[i],out buildingID);
                if (buildingID != 0)
                {
                    hotbar.BuildBuilding(buildingID, i / SectorSize, i % SectorSize);
                }
            }
            AddLog("Loaded Buildings in active sector", Priority.DEBUG);

            world.sectors[activeSectorX, activeSectorY] = loadedSector;
            ActiveSector = loadedSector;


            AddLog("Finished loading", Priority.DEBUG);

            MenuState = MenuStates.InGame;
        }
    }
}