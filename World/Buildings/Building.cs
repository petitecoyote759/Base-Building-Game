using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;



#pragma warning disable CS8603



namespace Base_Building_Game
{
    public static partial class General
    {
        // 1. Add to enum
        // 2. Make the class (inherit from Building or FBuilding if it is functional)
        // 3. Add to building images (add image if neccessary) (Renderer\Dictionaries.cs)
        // 4. Add building to the switch statement in Player\Building.cs\BuildingIDToBuilding












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
            WaterPump,

            SmallPort,
            MedPort,
            LargePort,

            
        }








        public interface Building : IEntity, IHasHealth
        {
            /// <summary>
            /// A function that determins what tiles it is valid to be placed on
            /// </summary>
            public Func<Tile, bool> ValidTiles { get; }
            public short ID { get; }
            public Inventory? inventory { get; set; }
            public int xSize { get; }
            public int ySize { get; }


            /// <summary>
            /// Rotation of the building, 0 is up, 1 is right, and so on.
            /// </summary>
            public int rotation { get; set; }

            public bool rotatable { get; }
        }





        public interface FBuilding : Building
        {
            public void Action(int dt);
        }

        
        public static Building BuildingIDToBuilding(BuildingID building,IVect pos)
        {
            return building switch
            {
                BuildingID.Bridge => new Bridge(pos),
                BuildingID.Wall => new Wall(pos),
                BuildingID.Extractor => new Extractor(pos),
                BuildingID.WorkCamp => new WorkCamp(pos),
                BuildingID.DropPod => new DropPod(pos),
                BuildingID.SmallPort => new SmallPort(pos, player.CurrrentRotation),
                BuildingID.MedPort => new MediumPort(pos, player.CurrrentRotation),
                BuildingID.LargePort => new LargePort(pos, player.CurrrentRotation),
                BuildingID.WaterPump => new WaterPump(pos),
                BuildingID.Barrel => new Barrel(pos),
                BuildingID.Pipe => new Pipe(pos),

                _ => null
            } ; // Add new buildings here ^^^^^^
        }






        public interface ConnectingBuilding : Building
        {
            public Func<Tile, bool> Connections { get; }
            public IntPtr connectionImage { get; }
        }
    }
}
