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
                float px = player.camPos.X;
                float py = player.camPos.Y;

                int Left = (int)((px) - (halfscreenwidth / zoom) - 2);
                int Top = (int)((py) - (halfscreenheight / zoom) - 2);
                // OPTIMISE: make it so it doesnt recalc every time 

                for (int x = Left; x < Left + screenwidth / zoom + 4; x++)
                {
                    for (int y = Top; y < Top + screenheight / zoom + 4; y++)
                    {
                        Tile tile = world.GetTile(x, y);

                        if (TileImages.ContainsKey(tile.ID))
                        {
                            DrawBP(x, px, y, py, TileImages[tile.ID], zoom, zoom);
                        }
                    }
                }
            }

            public void DrawEntities()
            {
                int zoom = renderer.zoom;
                float px = player.camPos.X;
                float py = player.camPos.Y;



                IEntity[] entities = LoadedEntities.ToArray();
                //This returns all of the entities which are on the screen using LINQ. It orders them by distance from the player in order to give them rendering priority if there are too many entities.
                IEntity[] entitiesToRender =
                    (from entity in entities
                     where GetPx(entity.pos.X) >= -zoom && GetPx(entity.pos.X) <= screenwidth && GetPy(entity.pos.Y) >= -zoom && GetPy(entity.pos.Y) <= screenheight
                     orderby Vector2.Dot(player.pos - entity.pos, player.pos - entity.pos) ascending
                     select entity).ToArray();

                IActiveEntity[] activeEntities = LoadedActiveEntities.ToArray();
                //This returns all of the entities which are on the screen using LINQ. It orders them by distance from the player in order to give them rendering priority if there are too many entities.
                IEntity[] activeEntitiesToRender =
                    (from entity in activeEntities
                     where GetPx(entity.pos.X) >= -zoom && GetPx(entity.pos.X) <= screenwidth && GetPy(entity.pos.Y) >= -zoom && GetPy(entity.pos.Y) <= screenheight
                     orderby Vector2.Dot(player.pos - entity.pos, player.pos - entity.pos) ascending
                     select entity).ToArray();


                //Iterating through all of the onscreen entities.
                foreach (IEntity entity in entitiesToRender)
                {
                    //If its an item, then we render using the itemImages dictionary.
                    if (entity is Item)
                    {
                        
                        Item item = (Item)entity;
                        //DrawBP(entity.pos.x / 32, entity.pos.y / 32, ItemImages[(short)item.ID]);
                        DrawBP(entity.pos.X, entity.pos.Y, ItemImages[item.ID]);
                    }
                    
                }
                foreach (IActiveEntity activeEntity in activeEntitiesToRender)
                {
                    if (activeEntity is Men)
                    {
                        DrawPP(activeEntity.pos.X, activeEntity.pos.Y, images["Man"]);
                    }
                    //When new entities are added, add them here:
                    //
                    if (activeEntity is Boat boat)
                    {
                        if (boat.ID == (short)BoatID.Skiff)
                        {
                            DrawPP(
                                boat.pos.X - (boat.Width / 2), 
                                boat.pos.Y - (boat.Length / 2), 
                                BoatImages[boat.ID][BoatResearch[boat.ID]], 
                                zoom * boat.Width, 
                                zoom * boat.Length, 
                                boat.angle);

                        }
                    }
                }
            }
            public void DrawBuildings()
            {
                int zoom = renderer.zoom;
                float px = player.camPos.X;
                float py = player.camPos.Y;

                int Left = (int)((px) - (halfscreenwidth / zoom) - 4);
                int Top = (int)((py) - (halfscreenheight / zoom) - 4);




                for (int x = Left; x < Left + screenwidth / zoom + 8; x++)
                {
                    for (int y = Top; y < Top + screenheight / zoom + 8; y++)
                    {
                        Tile tile = world.GetTile(x, y);
                        lock(tile)
                        {
                            if (tile.building is null) { continue; }

                            if (BuildingImages.ContainsKey(tile.building.ID) && tile.building is not Linker)
                            {
                                DrawBP(x, y,
                                    BuildingImages[tile.building.ID][Research[tile.building.ID]],
                                    zoom * tile.building.xSize,
                                    zoom * tile.building.ySize,
                                    tile.building.rotation * 90d
                                    );
                            }
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
