using System;
using System.Collections.Generic;
using System.Linq;
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

            int activeSectorX = worldJson.SectorX;
            int activeSectorY = worldJson.SectorY;
            

            Sector loadedSector = new Sector(true);
            SectorJson sectorData = worldJson.Sectors[activeSectorX, activeSectorY];
            


            
        }
    }
}