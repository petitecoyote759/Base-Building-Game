using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Debugger = Short_Tools.ShortDebugger;

namespace Base_Building_Game
{
    public static partial class General
    {
        public static Settings settings = new Settings();
        public static Debugger debugger = new Debugger("General");
    }
}
