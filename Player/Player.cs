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
        public class Player
        {
            public IVect pos = new IVect();
            public IVect camPos = new IVect();
            public IVect SectorPos = new IVect((SectorSize + 1) / 2, (SectorSize + 1) / 2);

            public Player()
            {

            }
        }
    }
}
