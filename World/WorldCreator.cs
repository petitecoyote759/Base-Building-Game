using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;


namespace Base_Building_Game
{
    public static partial class General
    {
        public static void CreateWorld()
        {
            world = new World();

            int size = World.size;

            world.sectors[(size + 1) / 2, (size + 1) / 2] = new Sector(true);

            ActiveSector = world[(World.size + 1) / 2, (World.size + 1) / 2];

            player.pos = new IVect(SectorSize / 2, SectorSize / 2);
            player.camPos = new IVect(SectorSize / 2, SectorSize / 2);
        }
    }
}
