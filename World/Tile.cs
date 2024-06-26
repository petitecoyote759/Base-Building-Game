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

            Error = short.MaxValue
        }



        // THE WIDTH OF A TILE IS 32!!!!!!!!!
        public class Tile
        {
            public short ID;
            public Building? building;


            public Tile()
            {
                ID = (short)TileID.Grass;
                building = null;
            }
        }
    }
}
