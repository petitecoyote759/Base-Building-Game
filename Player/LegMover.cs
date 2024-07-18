using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;
using static System.MathF;
using System.Numerics;

namespace Base_Building_Game
{
    public static partial class General
    {
        public static void MoveLegs(Vector2 UNDirection) // unnormalised direction
        {
            Vector2 Direction = UNDirection / UNDirection.Length();

            Leg ActiveLeg = player.Legs[player.CurrentMovingLeg];


            if (ActiveLeg.animation is PositionAnimation<Leg> anim)
            {
                if (!anim.Finished)
                {
                    return;
                }
                else
                {
                    ActiveLeg.animation = null;

                    ActiveLeg = player.Legs[(player.CurrentMovingLeg + 1) % player.Legs.Length];

                    for (float dist = Math.Min(
                        (player.x - ActiveLeg.x) / Direction.X,
                        (player.y - ActiveLeg.y) / Direction.Y); 
                        dist > 0; dist -= player.LegStep)
                    { 
                        if (!world.Walkable(ActiveLeg.pos + (Direction * dist))) { continue; }

                        renderer.CreateMoveAnimation<Leg>(ActiveLeg.pos + (Direction * dist), ActiveLeg, 4f, PositionAnimation<Leg>.Flags.Sigmoid);
                    }
                }
            }
        }














        public static bool InLegZones(Vector2 TestPos, Leg OGLeg)
        {
            float LDistSquared = player.LegDist * player.LegDist;

            foreach (Leg leg in player.Legs)
            {
                if (leg == OGLeg) { continue; }

                if ((leg - TestPos).LengthSquared() < LDistSquared) { return true; }
            }

            return false;
        }













        public static Vector2 GetIntersectionPoint(Leg ActiveLeg, bool top)
        {
            Vector2 p1 = ActiveLeg.pos;
            Vector2 p2 = player.pos;
            float r1 = player.LegDist;
            float r2 = player.JointDist;

            // Calculate the distance between the two circle centers
            float d = Vector2.Distance(p1, p2);

            // Check for solvability
            if (d > r1 + r2 || d < Math.Abs(r1 - r2) || (d == 0 && r1 == r2))
            {
                // No solutions, the circles do not intersect
                return new Vector2(-1, -1);
            }

            // 'a' is the distance from the first circle's center to the line joining the intersection points
            float a = (r1 * r1 - r2 * r2 + d * d) / (2 * d);

            // 'h' is the distance from the line joining the intersection points to the intersection points
            float h = MathF.Sqrt(r1 * r1 - a * a);

            // The point 'p' where the line through the circle intersection points crosses the line between the circle centers
            Vector2 p = p1 + a * (p2 - p1) / d;

            // Calculate the intersection points
            Vector2 intersection1 = new Vector2(
                p.X + h * (p2.Y - p1.Y) / d,
                p.Y - h * (p2.X - p1.X) / d
            );

            Vector2 intersection2 = new Vector2(
                p.X - h * (p2.Y - p1.Y) / d,
                p.Y + h * (p2.X - p1.X) / d
            );

            // Return one of the intersection points
            return top ? intersection1 : intersection2;
        }
    }
}
