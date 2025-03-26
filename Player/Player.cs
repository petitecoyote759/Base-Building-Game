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
            internal float currentSpeed = settings.PlayerSpeed;
            public int camspeed = 1;

            public Leg[] Legs = new Leg[] { 
                //new Leg()
            };
            public float LegDist = 1f;
            public float LegStep = 0.1f;
            public float LegMaxMove = 2f;
            public float JointDist = 0.5f;

            public int CurrentMovingLeg = 0;

            public float x { get => pos.X; set => pos = new Vector2(value, pos.Y); }
            public int blockX { get => (int)x; }
            public float y { get => pos.Y; set => pos = new Vector2(pos.X, value); }
            public int blockY { get => (int)y; }

            public double angle = 0d; // ik this is bad but you need it for SDL rotating

            public Player()
            {
                currentSpeed = speed;
            }









            public void Move(int dt)
            {
                if (world.GetTile(x, y).building?.ID == (short)BuildingID.Path)
                {
                    currentSpeed = this.speed * General.PathSpeedMultiplier;
                }
                else
                {
                    currentSpeed = this.speed;
                }

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



                Func<float, float, bool, bool> Walkable = world.Walkable;

                float speed = this.currentSpeed / 100f;


                if (boat is null && turret is null)
                {

                    if (ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_w])
                    {
                        if (ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_a] ^ ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_d]) { y -= speed * dt * 5 / 7; }
                        else { y -= speed * dt; }

                        if (!Walkable(x, y, true))
                        {
                            if (ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_a] ^ ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_d]) { y += speed * dt * 5 / 7; }
                            else { y += speed * dt; } // move back to the original place
                        }

                    }
                    if (ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_s])
                    {
                        if (ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_a] ^ ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_d]) { y += speed * dt * 5 / 7; }
                        else { y += speed * dt; }

                        if (!Walkable(x, y, true))
                        {
                            if (ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_a] ^ ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_d]) { y -= speed * dt * 5 / 7; }
                            else { y -= speed * dt; }
                        }

                    }
                    if (ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_a])
                    {
                        if (ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_w] ^ ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_s]) { x -= speed * dt * 5 / 7; }
                        else { x -= speed * dt; }

                        if (!Walkable(x, y, true))
                        {
                            if (ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_w] ^ ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_s]) { x += speed * dt * 5 / 7; }
                            else { x += speed * dt; }
                        }
                    }


                    if (ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_d])
                    {
                        if (ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_w] ^ ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_s]) { x += speed * dt * 5 / 7; }
                        else { x += speed * dt; }

                        if (!Walkable(x, y, true))
                        {
                            if (ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_w] ^ ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_s]) { x -= speed * dt * 5 / 7; }
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
