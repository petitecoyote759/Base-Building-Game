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
        public class SmallPort : Building
        {
            public Func<Tile, bool> ValidTiles { get; } = (Tile tile) => PrivateValidTiles(tile);

            static bool PrivateValidTiles(Tile tile)
            {
                if (tile.ID != (short)TileID.Ocean) { return false; }

                if (tile.building is SmallPort port)
                {
                    if (port.rotation == 0) { if (world.GetTile(port.pos.X, port.pos.Y + 1).ID == (short)TileID.Sand) { return true; } }
                    if (port.rotation == 1) { if (world.GetTile(port.pos.X - 1, port.pos.Y).ID == (short)TileID.Sand) { return true; } }
                    if (port.rotation == 2) { if (world.GetTile(port.pos.X, port.pos.Y - 1).ID == (short)TileID.Sand) { return true; } }
                    if (port.rotation == 3) { if (world.GetTile(port.pos.X + 1, port.pos.Y).ID == (short)TileID.Sand) { return true; } }
                }
                else
                {
                    // what
                    AddLog("Erm?", ShortDebugger.Priority.WARN);
                }
                return false;
            }






            public short ID { get; } = (short)BuildingID.SmallPort;
            public Inventory? inventory { get; set; } = new Inventory();
            public int CurrentHealth { get; set; }

            public int xSize { get; } = 1;
            public int ySize { get; } = 1;

            public Vector2 pos { get; set; }
            public int rotation { get; set; } = 0;
            public bool rotatable { get; } = true;

            public SmallPort(IVect pos, int rotation)
            {
                this.pos = pos;
                this.rotation = rotation;
            }
        }
    }
}
