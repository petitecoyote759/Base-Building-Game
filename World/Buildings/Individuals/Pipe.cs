using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Base_Building_Game.General;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;

namespace Base_Building_Game
{
    public static partial class General
    {
        public class Pipe : FBuilding, ConnectingBuilding
        {
            public Func<Tile, bool> ValidTiles { get; } = (Tile tile) =>
            {
                return tile.ID == (short)TileID.Grass || tile.ID == (short)TileID.Sand; // use a array to make more effic
            };

            public short ID { get; } = (short)BuildingID.Barrel;

            public int xSize { get; } = 1;
            public int ySize { get; } = 1;

            public Inventory? inventory { get; set; } = new Inventory();



            public int MaxHealth { get => Research[ID] * 1000 + 1000; } // edit this init
            public int CurrentHealth { get; set; }

            public Vector2 pos { get; set; }
            public int rotation { get; set; } = 0;
            public bool rotatable { get; } = false;

            public Func<Tile, bool> Connections { get; } = (Tile tile) => tile.building is not null && tile.building.ID == (short)BuildingID.Pipe;

            public IntPtr connectionImage
            {
                get => renderer.images["WallSegment" + Research[ID]];
            }

            public void Action(int dt)
            {
                
            }

            public Pipe(IVect pos)
            {
                this.pos = pos;
            }

        }


    }
}
