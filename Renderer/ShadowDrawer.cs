using Short_Tools;
using System.Numerics;
using static Base_Building_Game.General;
using static Short_Tools.General;
using static System.MathF;
using static System.Numerics.Vector2;
using IVect = Short_Tools.General.ShortIntVector2;
using Priority = Short_Tools.ShortDebugger.Priority;
using V2 = System.Numerics.Vector2;
using SDL2;
using static SDL2.SDL;



namespace Base_Building_Game
{
    public static partial class General
    {
        public partial class Renderer
        {
            public void DrawShadows()
            {
                float perc = Pow((Cos(2 * PI * (Time / (float)TimePerDay) + 0.25f) + 1) / 2f, 2);

                if (100 - 140d * perc > 0)
                {
                    foreach (var shadow in ShadowImages)
                    {
                        SDL_SetTextureAlphaMod(shadow.Value, (byte)(100 - 140d * perc));
                    }

                    SDL_SetTextureAlphaMod(images["Wall Bottom Shadow"], (byte)(100 - 140f * perc));
                    SDL_SetTextureAlphaMod(images["Wall Top Shadow"], (byte)(100 - 140f * perc));
                }


                SDL_SetTextureAlphaMod(renderer.images["Night Filter"], (byte)(140f * perc));





                float px = player.camPos.X;
                float py = player.camPos.Y;

                int Left = (int)((px) - (halfscreenwidth / zoom) - 4);
                int Top = (int)((py) - (halfscreenheight / zoom) - 4);




                for (int x = Left; x < Left + screenwidth / zoom + 8; x++)
                {
                    for (int y = Top; y < Top + screenheight / zoom + 8; y++)
                    {
                        Tile tempTile = world.GetTile(x, y);
                        if (tempTile.building is not null)
                        {
                            if (ShadowImages.ContainsKey(tempTile.building.ID))
                            {
                                DrawBuildingShadow(perc, px, py, x, y, tempTile.building);
                            }
                        }
                    }
                }


                Draw(0, 0, screenwidth, screenheight, "Night Filter"); // the filter for night time
            }





            private void DrawBuildingShadow(float TimePerc, float playerx, float playery, int x, int y, Building building)
            {
                if (TimePerc < 0)
                {
                    Draw(GetPx(x + 0.5f, playerx), GetPy(y, playery), (int)(zoom * TimePerc), zoom, ShadowImages[building.ID]);
                }
                else
                {
                    Draw(GetPx(x + 0.5f, playerx) - (int)(zoom * TimePerc), GetPy(y, playery), (int)(zoom * TimePerc), zoom, ShadowImages[building.ID]);
                }

                return;
                /*
                if (building.ID == (byte)BuildingID.Wall)
                {
                    Wall wall = (Wall)building;

                    if (ConnectingBuildings[(byte)BuildingID.Wall](Client.world.GetTile(x, y - 1)))
                    {
                        if ((int)(zoom * (float)Math.Cos(2 * Math.PI * Client.TimePerc)) < 0)
                        {
                            Draw(GetPx(x + 0.5f, playerx), GetPy(y, playery), (int)(zoom * 0.5f * MathF.Cos(2 * MathF.PI * (float)Client.TimePerc)), zoom, images["wall segment bottom"]);
                        }
                        else
                        {
                            Draw(GetPx(x + 0.5f, playerx) - (int)(zoom * 0.5f * MathF.Cos(2 * MathF.PI * (float)Client.TimePerc)), GetPy(y, playery), (int)(zoom * 0.5f * MathF.Cos(2 * MathF.PI * (float)Client.TimePerc)), zoom, images["wall segment bottom"]);
                        }
                    }
                    if (ConnectingBuildings[(byte)BuildingIDs.Wall](Client.world.GetTile(x, y + 1)))
                    {
                        if ((int)(zoom * (float)Math.Cos(2 * Math.PI * Client.TimePerc)) < 0)
                        {
                            Draw(GetPx(x + 0.5f, playerx), GetPy(y, playery), (int)(zoom * 0.5f * MathF.Cos(2 * MathF.PI * (float)Client.TimePerc)), zoom, images["wall segment top"]);
                        }
                        else
                        {
                            Draw(GetPx(x + 0.5f, playerx) - (int)(zoom * 0.5f * MathF.Cos(2 * MathF.PI * (float)Client.TimePerc)), GetPy(y, playery), (int)(zoom * 0.5f * MathF.Cos(2 * MathF.PI * (float)Client.TimePerc)), zoom, images["wall segment top"]);
                        }
                    }
                }
                */
            }
        }
    }
}