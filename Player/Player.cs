using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;
using MathI = Short_Tools.ShortMathI;
using static Short_Tools.General;
using Short_Tools;

namespace Base_Building_Game
{
    public static partial class General
    {
        public class Player : IEntity
        {
            public Vector2 pos { get; set; } = new Vector2();
            public Vector2 camPos = new Vector2();
            public IVect SectorPos = new IVect((World.size + 1) / 2, (World.size + 1) / 2);

            public IVect? selectedTile = null;
            public int CurrrentRotation = 0;

            public Boat? boat;
            public Turret? turret;
            public bool Piloting = false;

            public float speed { get => settings.PlayerSpeed; }
            public int camspeed = 1;

            public Leg[] Legs = new Leg[] { 
                new Leg(), new Leg(), new Leg(), new Leg()
            };
            public float LegDist = 3f;

            public float x { get => pos.X; set => pos = new Vector2(value, pos.Y); }
            public int blockX { get => (int)x; }
            public float y { get => pos.Y; set => pos = new Vector2(pos.X, value); }
            public int blockY { get => (int)y; }

            public double angle = 0d; // ik this is bad but you need it for SDL rotating

            public Player()
            {

            }









            public void Move(int dt)
            {
                Vector2 LastPos = pos;


                for (int i = 0; i < Legs.Length; i++)
                {
                    if ((Legs[i] - pos).Length() > LegDist * 10)
                    {
                        float angle = (float)(randy.NextDouble() * MathF.PI * 2);

                        Legs[i] = new Vector2(x + LegDist * MathF.Cos(angle), y + LegDist * MathF.Sin(angle));
                    }
                    else if (!world.Walkable((Vector2)Legs[i]))
                    {
                        float angle = (float)(randy.NextDouble() * MathF.PI * 2);

                        Legs[i] = new Vector2(x + LegDist * MathF.Cos(angle), y + LegDist * MathF.Sin(angle));
                    }
                }



                Func<int, int, bool, bool> Walkable = world.Walkable;

                float speed = this.speed / 100f;


                if (boat is null && turret is null)
                {

                    if (ActiveKeys["w"])
                    {
                        if (ActiveKeys["a"] ^ ActiveKeys["d"]) { y -= speed * dt * 5 / 7; }
                        else { y -= speed * dt; }

                        if (!Walkable(blockX, blockY, true))
                        {
                            if (ActiveKeys["a"] ^ ActiveKeys["d"]) { y += speed * dt * 5 / 7; }
                            else { y += speed * dt; } // move back to the original place
                        }

                    }
                    if (ActiveKeys["s"])
                    {
                        if (ActiveKeys["a"] ^ ActiveKeys["d"]) { y += speed * dt * 5 / 7; }
                        else { y += speed * dt; }

                        if (!Walkable(blockX, blockY, true))
                        {
                            if (ActiveKeys["a"] ^ ActiveKeys["d"]) { y -= speed * dt * 5 / 7; }
                            else { y -= speed * dt; }
                        }

                    }
                    if (ActiveKeys["a"])
                    {
                        if (ActiveKeys["w"] ^ ActiveKeys["s"]) { x -= speed * dt * 5 / 7; }
                        else { x -= speed * dt; }

                        if (!Walkable(blockX, blockY, true))
                        {
                            if (ActiveKeys["w"] ^ ActiveKeys["s"]) { x += speed * dt * 5 / 7; }
                            else { x += speed * dt; }
                        }
                    }


                    if (ActiveKeys["d"])
                    {
                        if (ActiveKeys["w"] ^ ActiveKeys["s"]) { x += speed * dt * 5 / 7; }
                        else { x += speed * dt; }

                        if (!Walkable(blockX, blockY, true))
                        {
                            if (ActiveKeys["w"] ^ ActiveKeys["s"]) { x -= speed * dt * 5 / 7; }
                            else { x -= speed * dt; }
                        }
                    }

                }

                if (boat is null)
                {
                    angle = (Math.PI / 2d + Math.Atan2((getMousePos().y - (renderer.GetPy(y))), (getMousePos().x - (renderer.GetPx(x))))) * 180d / Math.PI;
                }



                camPos = pos;


                //float ratio = camspeed * (float)(Math.ReciprocalSqrtEstimate(Math.Pow((x - campos.X), 2) + Math.Pow((y - campos.Y), 2))) * dt / 1000f;
                //float ratio = (dt * (128 + MathF.Pow((x - camPos.x) / 8, 2) + MathF.Pow((y - camPos.y) / 8, 2)) / 10000f);
                ////Print(ratio + " , " + pos + " : " + camPos);
                //
                //camPos.x = (int)((x - camPos.x) * ratio) + camPos.x;
                //camPos.y = (int)((y - camPos.y) * ratio) + camPos.y;

                // OPTIMISE: remove these ugly ass floats


                if (player.pos != LastPos)
                {
                    MoveLegs(player.pos - LastPos);
                }
            }






        }
    }
}
