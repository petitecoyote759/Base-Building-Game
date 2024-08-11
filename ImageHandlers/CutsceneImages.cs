using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;



namespace Base_Building_Game
{
    public static partial class General
    {
        public static Dictionary<string, IntPtr> CutsceneImages = new Dictionary<string, IntPtr>();

        public static Dictionary<string, string> CutsceneImagePaths = new Dictionary<string, string>()
        {
            { "TestImage0", "Cutscenes\\IntroCutscene\\Background.png" }
        };
    }
}
