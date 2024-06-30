using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        public enum ResearchLevel : byte
        {
            Wood,
            Advanced,
            Modern,
            HighTek,
            DragonsBreath
        }







        public static Dictionary<short, byte> Research = DefaultResearch();

        public static Dictionary<short, byte> DefaultResearch()
        {
            Dictionary<short, byte> Temp = new Dictionary<short, byte>();

            foreach (short id in Enum.GetValues<BuildingID>())
            {
                Temp.Add(id, (byte)ResearchLevel.Wood);
            }

            return Temp;
        }
    }
}
