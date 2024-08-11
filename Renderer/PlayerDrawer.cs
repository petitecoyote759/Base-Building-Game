using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Base_Building_Game.General;
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

                    Vector2 IPos = GetIntersectionPoint(leg, true);
                    if (IPos == new Vector2(-1, -1)) { IPos = player.pos; }


                    DrawTextureBetweenPoints(images["PlayerLeg"], 
                        GetPx(IPos.X), GetPy(IPos.Y), GetPx(player.x - 0.125f), GetPy(player.y - 0.125f),
                        zoom / 4, zoom / 4);
                    
                    DrawTextureBetweenPoints(images["PlayerLeg"],
                        GetPx(leg.X), GetPy(leg.Y), GetPx(IPos.X + 0.125f), GetPy(IPos.Y + 0.125f),
                        zoom / 4, zoom / 4);


                    Draw(
                        GetPx(leg.X) - (int)(zoom * player.LegDist), 
                        GetPy(leg.Y) - (int)(zoom * player.LegDist), 
                        (int)(zoom * 2 * player.LegDist), (int)(zoom * 2 * player.LegDist), 
                        "Circle");
                }
                //Draw(
                //        GetPx(player.x) - (int)(zoom * player.JointDist),
                //        GetPy(player.y) - (int)(zoom * player.JointDist),
                //        (int)(zoom * 2 * player.JointDist), (int)(zoom * 2 * player.JointDist),
                //        "Circle");




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
