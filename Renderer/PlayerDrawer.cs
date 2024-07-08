using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
                foreach (Vector2 leg in player.Legs)
                {
                    DrawTextureBetweenPoints(images["PlayerLeg"], 
                        GetPx(leg.X), GetPy(leg.Y), GetPx(player.x - 0.125f), GetPy(player.y - 0.125f),
                        zoom / 4, zoom / 4);
                }



                Draw(
                    (int)((player.x - player.camPos.X - 0.25f) * zoom + halfscreenwidth),
                    (int)((player.y - player.camPos.Y - 0.25f) * zoom + halfscreenheight), 
                    zoom  / 2, zoom / 2, "Player", player.angle);


                //DrawBP(
                //    (player.x - (player.LegDist)),
                //    (player.y - (player.LegDist)),
                //    "Circle",
                //    (int)(player.LegDist * zoom * 2), (int)(player.LegDist * zoom * 2));
            }
        }
    }
}
