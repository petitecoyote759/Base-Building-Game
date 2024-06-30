using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;

namespace Base_Building_Game
{
    public static partial class General
    {
        public class Linker : Building
        {
            public IVect pos { get; set; }

            public Func<Tile, bool> ValidTiles => connectedBuilding.ValidTiles;

            public int CurrentHealth
            {
                get
                {
                    return connectedBuilding.CurrentHealth; 
                }
                set
                {
                    connectedBuilding.CurrentHealth = value;
                }
            }

            public short ID
            {
                get
                {
                    return connectedBuilding.ID;
                }
            }

            public Inventory? inventory
            {
                get
                {
                    return connectedBuilding.inventory;
                }
                set
                {
                    connectedBuilding.inventory = value;
                }
            }


            public int xSize { get; } = 1;

            public int ySize  { get; } = 1;
            public int rotation { get; set; } = 0;
            public bool rotatable { get; } = false;

            public Building connectedBuilding;

            public Linker(IVect pos, Building connectedBuilding)
            {
                this.pos = pos;
                this.connectedBuilding = connectedBuilding;
            }
            public static void ClearLinkers(IVect pos, int xSize, int ySize)
            {
                IVect topLeft = pos;
                for (int x = 0; x < xSize; x++)
                {
                    for (int y = 0; y < ySize; y++)
                    {
                        ActiveSector[topLeft.x + x, topLeft.y + y].building = null;
                    }
                }
            }
        }
    }

    
}
