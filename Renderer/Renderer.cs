using Short_Tools;
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
        public partial class Renderer : XXShortRenderer
        {
            public Cutscene? ActiveCutscene = null;


            /// <summary>
            /// The size of tiles in pixils, used by the Renderer class.
            /// Make sure it doesnt go to 0, tbh 1 is also pretty bad, just make sure its like above
            /// 10 ish
            /// </summary>
            public int zoom = 64;

            public int halfscreenwidth = 960;
            public int halfscreenheight = 540;

#pragma warning disable CS8618 // must contain non null value -> its defined in load which is called immediately
#if DEBUG
            public Renderer() : base("Logs\\", Flag.Debug)
#else
            public Renderer() : base("Logs\\")
#endif
            { halfscreenwidth = screenwidth / 2; halfscreenheight = screenheight / 2; CheckSDLErrors(); }
#pragma warning restore CS8618


            public override void Render()
            {
                if (!InGame)
                {
                    RenderClear();

                    Draw(0, 0, screenwidth, screenheight, "Short Studios Logo");
                }
                else
                {
                    if (ActiveCutscene is Cutscene cutscene)
                    {
                        Draw(0, 0, screenwidth, screenheight, cutscene.CurrentFrame);
                    }
                    else
                    {
                        DrawTiles();
                        DrawShadows();
                        DrawBuildings();
                        DrawEntities();
                        DrawPlayer();


                        DrawUI();
                    }
                }

                RenderDraw();
            }













            


            








            /// <summary>
            /// Draws based on block position.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="image"></param>
            public void DrawBP(float x, float y, string image, double angle = 0)
            {
                DrawBP(x, y, images[image], angle);
            }
            /// <summary>
            /// Draws based on block position.
            /// </summary>
            public void DrawBP(float x, float y, IntPtr image, double angle = 0)
            {
                Draw(GetPx(x), GetPy(y), zoom, zoom, image, angle);
            }
            /// <summary>
            /// Draws based on block position.
            /// </summary>
            public void DrawBP(float x, float px, float y, float py, string image)
            {
                DrawBP(x, px, y, py, images[image]);
            }
            /// <summary>
            /// Draws based on block position.
            /// </summary>
            public void DrawBP(float x, float px, float y, float py, IntPtr image)
            {
                Draw(GetPx(x, px), GetPy(y, py), zoom, zoom, image);
            }

            /// <summary>
            /// Draws based on block position
            /// </summary>
            public void DrawBP(float x, float y, IntPtr image, int sizex, int sizey, double angle = 0d)
            {
                Draw(GetPx(x), GetPy(y), sizex, sizey, image, angle);
            }
            /// <summary>
            /// Draws based on block position
            /// </summary>
            public void DrawBP(float x, float y, string image, int sizex, int sizey, double angle = 0d)
            {
                DrawBP(x, y, images[image], sizex, sizey, angle);
            }
            /// <summary>
            /// Draws based on block position
            /// </summary>
            public void DrawBP(float x, float px, float y, float py, IntPtr image, int sizex, int sizey, double angle = 0d)
            {
                Draw(GetPx(x, px), GetPy(y, py), sizex, sizey, image, angle);
            }






            /// <summary>
            /// Draws based on the player position (anything scaled up by 32)
            /// </summary>
            public void DrawPP(float x, float y, IntPtr image, double angle = 0)
            {
                Draw(
                    (int)((x - player.camPos.X - 0.5f) * zoom + halfscreenwidth),
                    (int)((y - player.camPos.X - 0.5f) * zoom + halfscreenheight),
                    zoom, zoom, image, angle);
            }
            /// <summary>
            /// Draws based on the player position (anything scaled up by 32)
            /// </summary>
            public void DrawPP(float x, float y, string image, double angle = 0)
            {
                DrawPP(x, y, images[image], angle);
            }

            public void DrawPP(float x, float y, IntPtr image, int sizex, int sizey, double angle = 0)
            {
                Draw(
                    (int)((x - player.camPos.X - 0.5f) * zoom + halfscreenwidth),
                    (int)((y - player.camPos.Y - 0.5f) * zoom + halfscreenheight),
                    sizex, sizey, image, angle);
            }
            public void DrawPP(float x, float y, string image, int sizex, int sizey, double angle = 0)
            {
                DrawPP(x, y, images[image], sizex, sizey, angle);
            }






                /// <summary>
                /// Gets the pixil x position of an object. 
                /// </summary>
                /// <param name="x"></param>
                /// <returns></returns>
            public int GetPx(float x)
            {
                return (int)(zoom *  (x - player.camPos.X) + halfscreenwidth);
            }
            public int GetPx(float x, float px)
            {
                return (int)(zoom * (x - px) + halfscreenwidth);
            }


            /// <summary>
            /// Gets the pixil y position of an object. 
            /// </summary>
            /// <param name="y"></param>
            /// <returns></returns>
            public int GetPy(float y)
            {
                return (int)(zoom * (y - player.camPos.Y) + halfscreenheight);
            }
            public int GetPy(float y, float py)
            {
                return (int)(zoom * (y - py) + halfscreenheight);
            }
        }
    }
}
