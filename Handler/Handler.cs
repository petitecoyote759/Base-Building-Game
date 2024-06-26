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
        static Dictionary<string, bool> ActiveKeys = new Dictionary<string, bool>()
        {
            { "w", false },
            { "a", false },
            { "s", false },
            { "d", false },
        }; // the keys currently being pressed





            public class Handler : ShortHandler
            {

            public Handler() : base() { } // stick in base(Flag.Debug) to see what buttons are pressed 

            public override void Handle(string inp, bool down)
            {
                if (ActiveKeys.ContainsKey(inp)) { ActiveKeys[inp] = down; }



                switch (inp)
                {
                    case "SPACE":

                        if (!InGame) { InGame = true; }

                        break;
                }
            }
        }
    }
}
