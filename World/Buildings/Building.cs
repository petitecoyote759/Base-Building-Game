using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        // 1. Add to enum
        // 2. Make the class (inherit from Building or FBuilding if it is functional)
        // 3. Add to building images (add image if neccessary) (Renderer\Dictionaries.cs)
        // 4. Add building to the Hotbar switch statement in Player\Hotbar.cs\BuildBuildings()












        public enum BuildingID : short
        {
            None,


            Bridge,
            Wall,
            Extractor,
            WorkCamp,
            DropPod,
            ResearchLab,
            Barrel,
            Turret,
            Pipe,
            FloatingPlatform,
            OilRig,
            ConveyorBelt,
            Constructor,
        }








        public interface Building
        {
            public Func<Tile, bool> ValidTiles { get; }
            public int CurrentHealth { get; set; }
            public short ID { get; }
            public Inventory? inventory { get; set; }
            public int xSize { get; }
            public int ySize { get; }
        }





        public interface FBuilding : Building
        {
            public void Action(int dt);
        }
    }
}
