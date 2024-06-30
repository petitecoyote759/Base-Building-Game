using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        public static void WorldDeloader()
        {
            for (int i = 0; i < World.size; i++) 
            {
                for (int j = 0; j < World.size; j++) 
                {
                    world.sectors[i, j].map = null;
                }
            }


        }

    }
}
