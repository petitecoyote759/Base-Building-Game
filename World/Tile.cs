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
        public enum TileID : short
        {
            DeepOcean = 1,
            Ocean,
            Grass,
            Sand,
            Iron,
            Oil,
            Stone,
            Wood,
            Diamond,


            Error = short.MaxValue
        }



        // THE WIDTH OF A TILE IS 32!!!!!!!!!
        public class Tile
        {
            public int code = 255;

            public short ID;
            public Building? building;

            public Tile(bool random = false)
            {
                if (random)
                {
                    ID = (short)randy.Next(0, 4);
                }
                else
                {
                    ID = (short)TileID.Grass;
                }
                building = null;
            }

            public Tile(TileID ID)
            {
                this.ID = (short)ID;
            }
            public Tile(short ID)
            {
                this.ID = ID;
            }
        }



        public static int CalcCode(int x, int y)
        {
            IVect[] directions = new IVect[]
            {
            new IVect { x = x - 1, y = y - 1 },
            new IVect { x = x    , y = y - 1 },
            new IVect { x = x + 1, y = y - 1 },

            new IVect { x = x + 1, y = y     },
            new IVect { x = x + 1, y = y + 1 },

            new IVect { x = x    , y = y + 1 },
            new IVect { x = x - 1, y = y + 1 },
            new IVect { x = x - 1, y = y     }
            };







            int code = 0;

            switch (world.GetTile(x, y).ID)
            {
                // TODO: add calc code
                case (short)TileID.Sand:

                    foreach (IVect pos in directions)
                    {
                        if (world.GetTile(pos.x, pos.y).ID == (short)TileID.Ocean)
                        {
                            code += 1 << Array.IndexOf(directions, pos);
                        }
                    }


                    // if (topleft) { code += 1 << 0; }
                    // if (top) { code += 1 << 1; }
                    // if (topright) { code += 1 << 2; }
                    // if (right) { code += 1 << 3; }
                    // if (bottomright) { code += 1 << 4; }
                    // if (bottom) { code += 1 << 5; }
                    // if (bottomleft) { code += 1 << 6; }
                    // if (left) { code += 1 << 7; }

                    break;
            }

            return code;
        }


















        // ~~~ TILE FORMAT ~~~
        // Friendly? BuildingID
        //        ↑  <------->
        // 0000 0000 0000 0000
        // <-----> ↓
        // TileID  Floating?

        public static char ConvertTileToCharacter (Tile tile)
        {
            ushort Data = 0;
            Data += (ushort)((tile.building is null or Linker) ? 0 : tile.building.ID);
            Data += (ushort)(0 << 8); // Change to tile.floating at later point
            Data += (ushort)(0 << 9); // Change to tile.friendly at later point
            Data += (ushort)(tile.ID << 10);
            return (char)Data;
        }

        public static Tile ConvertCharacterToTile(char character)
        {
            Tile NewTile = new Tile();

            short BuildingID = (short)(character & 0b0000_0000_1111_1111);
            bool Floating = ((character & 0b0000_0001_0000_0000) >> 8) == 1;
            bool Friendly = ((character & 0b0000_0010_0000_0000) >> 9) == 1;
            NewTile.ID = (short)((character & 0b1111_1100_0000_0000) >> 10);
            

            return NewTile;
        }
        public static Tile ConvertCharacterToTile(char character,out BuildingID ID)
        {
            Tile NewTile = new Tile();

            short BuildingID = (short)(character & 0b0000_0000_1111_1111);
            bool Floating = ((character & 0b0000_0001_0000_0000) >> 8) == 1;
            bool Friendly = ((character & 0b0000_0010_0000_0000) >> 9) == 1;
            NewTile.ID = (short)((character & 0b1111_1100_0000_0000) >> 10);
            ID = (BuildingID)BuildingID;

            return NewTile;
        }
    }
}
