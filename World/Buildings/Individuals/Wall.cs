using System;
using System.Collections.Generic;
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
        public class Wall : ConnectingBuilding
        {
            public Func<Tile, bool> ValidTiles { get; } = (Tile tile) => 
            {
                return tile.ID == (short)TileID.Grass || tile.ID == (short)TileID.Sand; // use a array to make more effic
            };


            public Func<Tile, bool> Connections { get; } = (Tile tile) => tile.building is not null && tile.building.ID == (short)BuildingID.Wall;


            public IntPtr connectionImage 
            { 
                get => renderer.images["WallSegment" + Research[ID]];
            }







            public short ID { get; } = (short)BuildingID.Wall;
            public Inventory? inventory { get; set; } = null;
            public int CurrentHealth { get; set; }
            // maybe save connections?

            public int xSize { get; } = 1;
            public int ySize { get; } = 1;

            public IVect pos { get; set; }
            public int rotation { get; set; } = 0;
            public bool rotatable { get; } = false;


            public Wall(IVect pos)
            {
                this.pos = pos;
            }
        }
    }
}
