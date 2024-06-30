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
                    DrawBuildings();
                    DrawEntities();
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

            public void DrawEntities()
            {
                int zoom = renderer.zoom;
                int px = player.camPos.x;
                int py = player.camPos.Y;



                IEntity[] entities = LoadedEntities.ToArray();
                //This returns all of the entities which are on the screen using LINQ. It orders them by distance from the player in order to give them rendering priority if there are too many entities.
                IEntity[] entitiesToRender =
                    (from entity in entities
                     where GetPx(entity.pos.x / 32) >= -zoom && GetPx(entity.pos.x / 32) <= screenwidth && GetPy(entity.pos.y / 32) >= -zoom && GetPy(entity.pos.y / 32) <= screenheight
                     orderby (player.pos - entity.pos).Mag() ascending
                     select entity).ToArray();


                //Iterating through all of the onscreen entities.
                foreach (IEntity entity in entitiesToRender)
                {
                    //If its an item, then we render using the itemImages dictionary.
                    if (entity.GetType() == typeof(Item))
                    {
                        
                        Item item = (Item)entity;
                        //DrawBP(entity.pos.x / 32, entity.pos.y / 32, ItemImages[(short)item.ID]);
                        DrawPP(entity.pos.x, entity.pos.y, ItemImages[(short)item.ID]);
                    }
                    //When new entities are added, add them here:
                    //
                }
            }
            public void DrawBuildings()
            {
                int zoom = renderer.zoom;
                int px = player.camPos.x;
                int py = player.camPos.y;

                int Left = (px / 32) - (halfscreenwidth / zoom) - 1;
                int Top = (py / 32) - (halfscreenheight / zoom) - 2;




                for (int x = Left; x < Left + screenwidth / zoom + 2; x++)
                {
                    for (int y = Top; y < Top + screenheight / zoom + 4; y++)
                    {
                        Tile tile = world.GetTile(x, y);

                        if (tile.building is null) { continue; }    

                        if (BuildingImages.ContainsKey(tile.building.ID))
                        {
                            DrawBP(x, y, 
                                BuildingImages[tile.building.ID][Research[tile.building.ID]],
                                zoom * tile.building.xSize,
                                zoom * tile.building.ySize
                                );
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
            /// <summary>
            /// Draws based on block position.
            /// </summary>
            public void DrawBP(int x, int y, IntPtr image)
            {
                Draw(GetPx(x), GetPy(y), zoom, zoom, image);
            }
            /// <summary>
            /// Draws based on block position.
            /// </summary>
            public void DrawBP(int x, int px, int y, int py, string image)
            {
                DrawBP(x, px, y, py, images[image]);
            }
            /// <summary>
            /// Draws based on block position.
            /// </summary>
            public void DrawBP(int x, int px, int y, int py, IntPtr image)
            {
                Draw(GetPx(x, px), GetPy(y, py), zoom, zoom, image);
            }

            /// <summary>
            /// Draws based on block position
            /// </summary>
            public void DrawBP(int x, int y, IntPtr image, int sizex, int sizey, double angle = 0d)
            {
                Draw(GetPx(x), GetPy(y), sizex, sizey, image);
            }
            /// <summary>
            /// Draws based on block position
            /// </summary>
            public void DrawBP(int x, int y, string image, int sizex, int sizey, double angle = 0d)
            {
                DrawBP(x, y, images[image], sizex, sizey, angle);
            }





            /// <summary>
            /// Draws based on the player position (anything scaled up by 32)
            /// </summary>
            public void DrawPP(int x, int y, IntPtr image, double angle = 0)
            {
                Draw(
                    (x - player.camPos.x - 16) * zoom / 32 + halfscreenwidth,
                    (y - player.camPos.y - 16) * zoom / 32 + halfscreenheight,
                    zoom, zoom, image, angle);
            }
            /// <summary>
            /// Draws based on the player position (anything scaled up by 32)
            /// </summary>
            public void DrawPP(int x, int y, string image, double angle = 0)
            {
                DrawPP(x, y, images[image], angle);
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
