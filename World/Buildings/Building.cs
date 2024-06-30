using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
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
            public void Action(long dt);
        }
    }
}
