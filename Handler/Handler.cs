using Short_Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;

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
            { "Mouse", false },
        }; // the keys currently being pressed





            public class Handler : ShortHandler
            {

            public Handler() : base() { } // stick in base(Flag.Debug) to see what buttons are pressed 

            public override void Handle(string inp, bool down)
            {
                if (ActiveKeys.ContainsKey(inp)) { ActiveKeys[inp] = down; }

                if (int.TryParse(inp, out int result)) // if it is a number
                {
                    HotbarSelected = result;
                }

                switch (inp)
                {
                    case "ESCAPE":

                        if (down)
                        {
                            HotbarSelected = -1;
                        }

                        break;


                    case "SPACE":

                        if (!InGame) { InGame = true; }  // TODO: change this when the menu is added
                        else
                        {
                            Item item = new Item(ItemID.Wood, player.pos);
                        }
                        break;

                    case "MouseWheel":

                        if (down)
                        {
                            renderer.zoom = renderer.zoom * 11 / 10;
                            if (renderer.zoom > 200) { renderer.zoom = 200; }
                        }
                        else
                        {
                            renderer.zoom = renderer.zoom * 9 / 10;
                            if (renderer.zoom < 10) { renderer.zoom = 10; }
                        }

                        break;

                    case "F1":

                        if (down)
                        {
                            settings.Debugging = !settings.Debugging;
                        }

                        break;
                    
                }
            }
        }
    }
}
