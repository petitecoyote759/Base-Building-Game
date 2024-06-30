using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;
using Short_Tools;
using static Short_Tools.General;


namespace Base_Building_Game
{
    public static partial class General
    {
        public partial class Renderer
        {
            public void DrawUI()
            {
                IVect MPos = getMousePos();
                DrawBP(GetBlockx(MPos.x), GetBlocky(MPos.y), "MouseBox");
                // MouseBox   return (zoom * x) - (zoom * player.camPos.x / 32) + halfscreenwidth;

                Write(0, 0, 50, 50, (player.pos / 32).ToString());
            }





            public int GetBlockx(int x)
            {
                return (x - halfscreenwidth + (zoom * player.camPos.x / 32)) / zoom;
            }
            public int GetBlocky(int y)
            {
                return (y - halfscreenheight + (zoom * player.camPos.y / 32)) / zoom;
            }
        }
    }
}
