using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        public static Dictionary<short, byte> Research = DefaultResearch();

        public static Dictionary<short, byte> DefaultResearch()
        {
            Dictionary<short, byte> Temp = new Dictionary<short, byte>();

            foreach (short id in Enum.GetValues<BuildingID>())
            {
                Temp.Add(id, 0);
            }

            return Temp;
        }
    }
}
