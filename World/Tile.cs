using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            public short ID;
            public Building? building;

            public Tile(bool random = false)
            {
                if (random)
                {
                    ID = (short)randy.Next(0,4);
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
