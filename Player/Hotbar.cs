using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;


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
                    (byte)BuildingID.Bridge    => new Bridge(new IVect(x, y)),
                    (byte)BuildingID.Wall      => new Wall(new IVect(x, y)),
                    (byte)BuildingID.Extractor => new Extractor(new IVect(x, y)),
                    (byte)BuildingID.DropPod => new DropPod(new IVect(x,y)),
                    _ => null
                }; // Add new buildings here ^^^^^^




                if (ActiveSector[x, y].building is null) { return; }

                if (!ActiveSector[x, y].building.ValidTiles(world.GetTile(x, y)))
                {
                    ActiveSector[x, y].building = null;
                    return;
                }

                if (ActiveSector[x, y].building.xSize > 1 || ActiveSector[x, y].building.ySize > 1)
                {
                    List<IVect> tempLinkers = new List<IVect>()
                    {
                        new IVect(x,y)
                    };
                    for (int i = 0; i < ActiveSector[x, y].building.xSize; i++)
                    {
                        for (int j = 0; j < ActiveSector[x, y].building.ySize; j++)
                        {
                            if (i == 0 && j == 0)
                            {
                                continue;
                            }
                            if (ActiveSector[x, y].building.ValidTiles(world.GetTile(x + i, y + j)) && ActiveSector[x+i,y+j].building is null)
                            {
                                tempLinkers.Add(new IVect(x + i, y + j));
                                ActiveSector[x+i,y+j].building = new Linker(new IVect(x+i, y + j), ActiveSector[x,y].building);
                            }
                            else
                            {
                                foreach (IVect tempLinker in tempLinkers)
                                {
                                    ActiveSector[tempLinker.x, tempLinker.y].building = null;  
                                    
                                }
                                return;
                            }
                        }
                    }

                }


                if (ActiveSector[x, y].building is FBuilding)
                {
                    FBuildings.Add((FBuilding)ActiveSector[x, y].building);
                }
            }
        }
    }
}
