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
            DeepOcean,
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
    }
}
