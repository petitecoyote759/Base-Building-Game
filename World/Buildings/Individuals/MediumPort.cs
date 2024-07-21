using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;



namespace Base_Building_Game
{
    public static partial class General
    {
        public class MediumPort : Building
        {
            public Func<Tile, bool> ValidTiles { get; } = (Tile tile) => PrivateValidTiles(tile);

            static bool PrivateValidTiles(Tile tile)
            {
                if (tile.ID != (short)TileID.Ocean) { return false; }

                //if (tile.building is MediumPort port)
                //{
                //    if (port.rotation == 0) { if (world.GetTile(port.pos.x, port.pos.y + 1).ID == (short)TileID.Sand) { return true; } }
                //    if (port.rotation == 1) { if (world.GetTile(port.pos.x - 1, port.pos.y).ID == (short)TileID.Sand) { return true; } }
                //    if (port.rotation == 2) { if (world.GetTile(port.pos.x, port.pos.y - 1).ID == (short)TileID.Sand) { return true; } }
                //    if (port.rotation == 3) { if (world.GetTile(port.pos.x + 1, port.pos.y).ID == (short)TileID.Sand) { return true; } }
                //}
                //else
                //{
                //    // what
                //    AddLog("Erm?", ShortDebugger.Priority.WARN);
                //}
                return true;
            }






            public short ID { get; } = (short)BuildingID.MedPort;
            public Inventory? inventory { get; set; } = new Inventory();
            public int CurrentHealth { get; set; }

            public int xSize { get; } = 3;
            public int ySize { get; } = 3;

            public Vector2 pos { get; set; }
            public int rotation { get; set; } = 0;
            public bool rotatable { get; } = true;

            public MediumPort(IVect pos, int rotation)
            {
                this.pos = pos;
                this.rotation = rotation;
            }
        }
    }
}
