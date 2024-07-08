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

            for (int i = 0; i < player.Legs.Length; i++)
            {
                if ((player.pos - player.Legs[i]).LengthSquared() > player.LegDist * player.LegDist / 8)
                {
                    if (Direction.X > 0 && player.Legs[i].X - player.x > 0) { continue; }
                    if (Direction.X < 0 && player.Legs[i].X - player.x < 0) { continue; }
                    if (Direction.Y > 0 && player.Legs[i].Y - player.y > 0) { continue; }
                    if (Direction.Y < 0 && player.Legs[i].Y - player.y < 0) { continue; }


                    Animation[] animations = renderer.animations.ToArray();
                    ISHasPosition[] CurrentAnimations = (from ani in animations select (ISHasPosition)ani.ObjGetter).ToArray();
                    if (CurrentAnimations.Contains(player.Legs[i])) { continue; }






                    float NewRad = (float)(randy.NextDouble() * player.LegDist / 4d) + (player.LegDist * 3f / 4f);

                    Func<Vector2, Vector2, float> Dot = Vector2.Dot;

                    Vector2 DeltaLeg = player.pos - player.Legs[i]; // P - L
                    float DistDeltaDot = Dot(Direction, DeltaLeg); // (P - L) * D


                    float Dist = DistDeltaDot +
                        Sqrt(
                            DistDeltaDot * DistDeltaDot
                            - Dot(DeltaLeg, DeltaLeg)
                            + (NewRad * NewRad));


                    //player.Legs[i] = player.Legs[i] + Dist * Direction;


                    renderer.CreateMoveAnimation(
                        player.Legs[i] + Dist * Direction,
                        player.Legs[i],
                        4f,
                        PositionAnimation<Leg>.Flags.Root
                        );

                    if (!world.Walkable((Vector2)player.Legs[i])) { player.Legs[i] = player.pos; }
                }
            }
        }
    }
}
