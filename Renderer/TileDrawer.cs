using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;




namespace Base_Building_Game
{
    public static partial class General
    {
        public partial class Renderer
        {
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

                        if (FancyTiles.ContainsKey(tile.ID))
                        {
                            int code = CalcCode(x, y);

                            DrawBP(x, px, y, py, FancyTiles[tile.ID][code], zoom, zoom);
                        }
                        else if (TileImages.ContainsKey(tile.ID))
                        {
                            DrawBP(x, px, y, py, TileImages[tile.ID], zoom, zoom);
                        }

                        //  "Night Filter"
                        const float altitudeRenderingMultiplier = 10f;

                        float deltaAltitude = tile.altitude - world.GetTile(player.pos.X, player.pos.Y).altitude;
                        float alphaStart = Math.Max(((5 - (deltaAltitude * altitudeRenderingMultiplier)) * 255f / 20f), 0);
                        byte alpha = (byte)Math.Min(alphaStart, 255f);
                        SDL2.SDL.SDL_SetTextureAlphaMod(images["Night Filter"], alpha);
                        // 0 - 255, i want lower altitudes to be higher alpha. altitude goes from -10 -> 10, so to get to 0 -> 255 i need (10 - A) * 255 / 20
                        DrawBP(x, px, y, py, images["Night Filter"], zoom, zoom);
                    }
                }
            }
        }
    }
}