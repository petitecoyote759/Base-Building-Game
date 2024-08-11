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
                float TimePerc = Time / (float)TimePerDay;



                double perc = Math.Pow((Math.Cos(2 * Math.PI * ((TimePerc + 0.25f))) + 1) / 2d, 2);

                if (100 - 140d * perc > 0)
                {
                    foreach (var shadow in ShadowImages)
                    {
                        SDL_SetTextureAlphaMod(shadow.Value, (byte)(100 - 140d * perc));
                    }

                    SDL_SetTextureAlphaMod(images["Wall Bottom Shadow"], (byte)(100 - 140d * perc));
                    SDL_SetTextureAlphaMod(images["Wall Top Shadow"], (byte)(100 - 140d * perc));
                }

                SDL_SetTextureAlphaMod(images["Night Filter"], (byte)(140d * perc));



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
                                DrawBuildingShadow(px, py, x, y, tempTile.building);
                            }
                        }
                    }
                }
            }





            private void DrawBuildingShadow(float playerx, float playery, int x, int y, Building building)
            {
                float TimePerc = Time / (float)TimePerDay;

                if ((int)(zoom * MathF.Cos(2 * MathF.PI * (float)TimePerc)) < 0)
                {
                    Draw(GetPx(x + 0.5f, playerx), GetPy(y, playery), (int)(zoom * MathF.Cos(2 * MathF.PI * (float)TimePerc)), zoom, ShadowImages[building.ID]);
                }
                else
                {
                    Draw(GetPx(x + 0.5f, playerx) - (int)(zoom * MathF.Cos(2 * MathF.PI * (float)TimePerc)), GetPy(y, playery), (int)(zoom * MathF.Cos(2 * MathF.PI * (float)TimePerc)), zoom, ShadowImages[building.ID]);
                }

                
                if (building.ID == (byte)BuildingID.Wall)
                {
                    Wall wall = (Wall)building;

                    if (wall.Connections(world.GetTile(x, y - 1)))
                    {
                        if ((int)(zoom * (float)Math.Cos(2 * Math.PI * TimePerc)) < 0)
                        {
                            Draw(GetPx(x + 0.5f, playerx), GetPy(y, playery), (int)(zoom * 0.5f * MathF.Cos(2 * MathF.PI * (float)TimePerc)), zoom, images["Wall Bottom Shadow"]);
                        }
                        else
                        {
                            Draw(GetPx(x + 0.5f, playerx) - (int)(zoom * 0.5f * MathF.Cos(2 * MathF.PI * (float)TimePerc)), GetPy(y, playery), (int)(zoom * 0.5f * MathF.Cos(2 * MathF.PI * (float)TimePerc)), zoom, images["Wall Bottom Shadow"]);
                        }
                    }
                    if (wall.Connections(world.GetTile(x, y + 1)))
                    {
                        if ((int)(zoom * (float)Math.Cos(2 * Math.PI * TimePerc)) < 0)
                        {
                            Draw(GetPx(x + 0.5f, playerx), GetPy(y, playery), (int)(zoom * 0.5f * MathF.Cos(2 * MathF.PI * (float)TimePerc)), zoom, images["Wall Top Shadow"]);
                        }
                        else
                        {
                            Draw(GetPx(x + 0.5f, playerx) - (int)(zoom * 0.5f * MathF.Cos(2 * MathF.PI * (float)TimePerc)), GetPy(y, playery), (int)(zoom * 0.5f * MathF.Cos(2 * MathF.PI * (float)TimePerc)), zoom, images["Wall Top Shadow"]);
                        }
                    }
                }
            }
        }
    }
}