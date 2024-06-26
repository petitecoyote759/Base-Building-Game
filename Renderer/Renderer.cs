using Short_Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    Print(DateTimeOffset.Now.ToUnixTimeMilliseconds());
                    DrawTiles();
                }



                RenderDraw();
            }











            public void DrawTiles()
            {
                // need to get pos of the top left part of the screen
                // players pos is at the centre of the screen, - zoom / 2
                // so in terms of blocks, for x that is 
                //   

                int Left = (player.pos.x / 32) - (halfscreenwidth / zoom);
                int Top = (player.pos.y / 32) - (halfscreenheight / zoom);
                // OPTIMISE: make it so it doesnt recalc every time 

                for (int x = 0; x < Left; x++)
                {
                    for (int y = 0; y < Top; y++)
                    {
                        Tile tile = world.GetTile(x, y);

                        if (TileImages.ContainsKey(tile.ID))
                        {
                            DrawBP(x, y, TileImages[tile.ID]);
                        }
                    }
                }
            }









            public void DrawBP(int x, int y, string image)
            {
                DrawBP(x, y, images[image]);
            }
            public void DrawBP(int x, int y, IntPtr image)
            {
                Draw(GetPx(x), GetPy(y), zoom, zoom, image);
            }



            public int GetPx(int x)
            {
                return zoom * (x - player.camPos.x) + halfscreenwidth;
            }
            public int GetPy(int y)
            {
                return zoom * (y - player.camPos.y) + halfscreenheight;
            }
        }
    }
}
