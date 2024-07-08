using Short_Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        public class Settings : ShortSettings
        {
            public bool Debugging { get; set; } = false;
            public bool Cheats { get; set; } = false;
            public float PlayerSpeed { get; set; } = 0.4f;











            public override string ToString()
            {
                return Short_Tools.General.GetDisplayString(this);
            }
        }
    }
}
