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
            public IVect pos = new IVect();
            public IVect camPos = new IVect();
            public IVect SectorPos = new IVect((World.size + 1) / 2, (World.size + 1) / 2);

            public IVect? selectedTile = null;

            public int speed { get => settings.PlayerSpeed; }
            public int camspeed = 1;

            public int x { get => pos.x; set => pos.x = value; }
            public int blockX { get => pos.x / 32; }
            public int y { get => pos.y; set => pos.y = value; }
            public int blockY { get => pos.y / 32; }

            public double angle = 0d; // ik this is bad but you need it for SDL rotating

            public Player()
            {

            }









            public void Move(int dt)
            {
                dt = dt * 3 / 5;
                Func<int, int, bool, bool> Walkable = world.Walkable;


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


                camPos = pos;

                angle = (Math.PI / 2d + Math.Atan2((getMousePos().y - (renderer.GetPy(y / 32))), (getMousePos().x - (renderer.GetPx(x / 32))))) * 180d / Math.PI;

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
