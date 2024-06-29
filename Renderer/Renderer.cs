using Short_Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Base_Building_Game.General;
using static Short_Tools.General;





namespace Base_Building_Game
{
    public static partial class General
    {
        public partial class Renderer : ShortRenderer
        {
            /// <summary>
            /// The size of tiles in pixils, used by the Renderer class.
            /// Make sure it doesnt go to 0, tbh 1 is also pretty bad, just make sure its like above
            /// 10 ish
            /// </summary>
            public int zoom = 64;

            public int halfscreenwidth = 960;
            public int halfscreenheight = 540;

#if DEBUG
            public Renderer() : base("Logs\\", Flag.Debug)
#else
            public Renderer() : base("Logs\\")
#endif
            { halfscreenwidth = screenwidth / 2; halfscreenheight = screenheight / 2; }

            public override void Render()
            {
                RenderClear();

                if (!InGame)
                {
                    //RenderClear();

                    Draw(0, 0, screenwidth, screenheight, "Short Studios Logo");
                }
                else
                {
                    DrawTiles();
                    DrawPlayer();
                    DrawUI();
                }



                RenderDraw();
            }











            public void DrawTiles()
            {
                // need to get pos of the top left part of the screen
                // players pos is at the centre of the screen, - zoom / 2
                // so in terms of blocks, for x that is 
                //   
                int zoom = renderer.zoom;
                int px = player.camPos.x;
                int py = player.camPos.y;

                int Left = (px / 32) - (halfscreenwidth / zoom) - 1;
                int Top = (py / 32) - (halfscreenheight / zoom) - 2;
                // OPTIMISE: make it so it doesnt recalc every time 

                for (int x = Left; x < Left + screenwidth / zoom + 2; x++)
                {
                    for (int y = Top; y < Top + screenheight / zoom + 4; y++)
                    {
                        Tile tile = world.GetTile(x, y);

                        if (TileImages.ContainsKey(tile.ID))
                        {
                            DrawBP(x, px, y, py, TileImages[tile.ID]);
                        }
                    }
                }
            }








            /// <summary>
            /// Draws based on block position.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="image"></param>
            public void DrawBP(int x, int y, string image)
            {
                DrawBP(x, y, images[image]);
            }
            public void DrawBP(int x, int y, IntPtr image)
            {
                Draw(GetPx(x), GetPy(y), zoom, zoom, image);
            }
            public void DrawBP(int x, int px, int y, int py, string image)
            {
                DrawBP(x, px, y, py, images[image]);
            }
            public void DrawBP(int x, int px, int y, int py, IntPtr image)
            {
                Draw(GetPx(x, px), GetPy(y, py), zoom, zoom, image);
            }






            /// <summary>
            /// Gets the pixil x position of an object. 
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            public int GetPx(int x)
            {
                return (zoom * x) - (zoom * player.camPos.x / 32) + halfscreenwidth;
            }
            public int GetPx(int x, int px)
            {
                return (zoom * x) - (zoom * px / 32) + halfscreenwidth;
            }


            /// <summary>
            /// Gets the pixil y position of an object. 
            /// </summary>
            /// <param name="y"></param>
            /// <returns></returns>
            public int GetPy(int y)
            {
                return (zoom * y) - (zoom * player.camPos.y / 32) + halfscreenheight;
            }
            public int GetPy(int y, int py)
            {
                return (zoom * y) - (zoom * py / 32) + halfscreenheight;
            }
        }
    }
}
