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

namespace Base_Building_Game
{
    public static partial class General
    {
        /// <summary>
        /// Loads the world, and active sector, from the specified JSON.
        /// </summary>
        /// <param name="path">The location of the JSON file containing the world data.</param>
        public static void LoadWorld(string path)
        {
           //Loads the world from the json file.

            WorldJson? worldJson = JsonConvert.DeserializeObject<WorldJson>(path);
            if (worldJson is null)
            {
                //Uh oh! Kind of an issue.
                debugger.AddLog("World JSON returned null");

                //This should return to menu. Add this later.
                return;
            }

            //Setting the position of the player.
            player.x = worldJson.PlayerX;
            player.y = worldJson.PlayerY;

            //Initializing the world.
            world = new World();

            //Getting the position of the active sector from the JSON.
            int activeSectorX = worldJson.SectorX;
            int activeSectorY = worldJson.SectorY;

            //Creating the sector we are going to load, and pulling the data from the JSON.
            Sector loadedSector = new Sector(true);
            SectorJson sectorData = worldJson.Sectors[activeSectorX, activeSectorY];
            
            //This will fill up the sector with the tile data from the bit structure.
            for (int x = 0; x < SectorSize; x++)
            {
                for (int y = 0; y < SectorSize; y++)
                {
                    //Converts the character stored in the JSON into a Tile.
                    loadedSector[x, y] = ConvertCharacterToTile(sectorData.MapData[x + y]);
                }
            }

            world.sectors[activeSectorX,activeSectorY] = loadedSector;
            
        }
    }
}