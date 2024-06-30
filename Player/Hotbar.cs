using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        public static int HotbarSelected = -1;


        public class Hotbar
        {
            short[] items = new short[10];

            public short this[int index] => items[index];

            public void SetBuilding(BuildingID ID, int pos = -1)
            {
                SetBuilding((short)ID, pos);
            }
            public void SetBuilding(short ID, int pos = -1) 
            { 
                if (pos < -1 || pos > 9) { pos = -1; }

                if (pos == -1)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (items[i] == (byte)BuildingID.None)
                        {
                            items[i] = ID;
                            return;
                        }
                    }
                    items[9] = ID;
                    return;
                }
                else
                {
                    items[pos] = ID;
                }
            }


            public void BuildBuilding(int pos, int x, int y)
            {
                if (ActiveSector[x, y].building is not null) { return; }




                ActiveSector[x, y].building = items[pos] switch
                {
                    (byte)BuildingID.Bridge    => new Bridge(),
                    (byte)BuildingID.Wall      => new Wall(),
                    (byte)BuildingID.Extractor => new Extractor(),

                    _ => null
                }; // Add new buildings here ^^^^^^




                if (ActiveSector[x, y].building is null) { return; }

                if (!ActiveSector[x, y].building.ValidTiles(world.GetTile(x, y)))
                {
                    ActiveSector[x, y].building = null;
                }
            }
        }
    }
}
