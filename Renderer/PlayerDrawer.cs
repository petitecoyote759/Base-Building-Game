using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Short_Tools.General;


namespace Base_Building_Game
{
    public static partial class General
    {
        public partial class Renderer
        {
            public void DrawPlayer()
            {
                //Draw(
                //    (player.x - player.camPos.x - 16) * zoom / 32 + halfscreenwidth,
                //    (player.y - player.camPos.y  -16) * zoom / 32 + halfscreenheight, 
                //    zoom, zoom, "Player", player.angle);
                DrawPP(player.x, player.y, "Player", player.angle);
            }
        }
    }
}
