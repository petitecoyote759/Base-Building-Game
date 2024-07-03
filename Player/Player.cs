using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;
using MathI = Short_Tools.ShortMathI;
using static Short_Tools.General;


namespace Base_Building_Game
{
    public static partial class General
    {
        public class Player
        {
            public Vector2 pos = new Vector2();
            public Vector2 camPos = new Vector2();
            public IVect SectorPos = new IVect((World.size + 1) / 2, (World.size + 1) / 2);

            public IVect? selectedTile = null;
            public int CurrrentRotation = 0;

            public Boat? boat;
            public bool Piloting = false;

            public float speed { get => settings.PlayerSpeed; }
            public int camspeed = 1;

            public float x { get => pos.X; set => pos.X = value; }
            public int blockX { get => (int)x; }
            public float y { get => pos.Y; set => pos.Y = value; }
            public int blockY { get => (int)y; }

            public double angle = 0d; // ik this is bad but you need it for SDL rotating

            public Player()
            {

            }









            public void Move(int dt)
            {
                Func<int, int, bool, bool> Walkable = world.Walkable;

                float speed = this.speed / 50f;


                if (!Piloting)
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


                    angle = (Math.PI / 2d + Math.Atan2((getMousePos().y - (renderer.GetPy(blockY))), (getMousePos().x - (renderer.GetPx(blockX))))) * 180d / Math.PI;

                }


                camPos = pos;


                //float ratio = camspeed * (float)(Math.ReciprocalSqrtEstimate(Math.Pow((x - campos.X), 2) + Math.Pow((y - campos.Y), 2))) * dt / 1000f;
                //float ratio = (dt * (128 + MathF.Pow((x - camPos.x) / 8, 2) + MathF.Pow((y - camPos.y) / 8, 2)) / 10000f);
                ////Print(ratio + " , " + pos + " : " + camPos);
                //
                //camPos.x = (int)((x - camPos.x) * ratio) + camPos.x;
                //camPos.y = (int)((y - camPos.y) * ratio) + camPos.y;

                // OPTIMISE: remove these ugly ass floats
            }
        }
    }
}
