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
        public class Renderer : ShortRenderer
        {
            public Renderer() : base("Logs\\") { }

            public override void Render()
            {
                RenderClear();

                if (!InGame)
                {
                    Draw(0, 0, screenwidth, screenheight, "Short Studios Logo");
                }



                RenderDraw();
            }
        }
    }
}
