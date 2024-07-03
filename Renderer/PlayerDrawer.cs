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
                Draw(
                    (int)((player.x - player.camPos.X - 0.25f) * zoom + halfscreenwidth),
                    (int)((player.y - player.camPos.Y - 0.25f) * zoom + halfscreenheight), 
                    zoom  / 2, zoom / 2, "Player", player.angle);
                //DrawPP(player.x - 16, player.y - 16, "Player", player.angle);
            }
        }
    }
}
