using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;


namespace Base_Building_Game
{
    public static partial class General
    {
        public static Dictionary<int, IntPtr[]> FancyTiles = new Dictionary<int, IntPtr[]>()
        {

        };


        public static void LoadFancyTiles()
        {
            createImages(renderer.images["GrassSS"], (int)TileID.Grass, 95, 95);
            createImages(renderer.images["SandSS"], (int)TileID.Sand, 96, 96);
            createImages(renderer.images["OceanSS"], (int)TileID.Ocean, 96, 96);
        }












        public static void createImages(IntPtr spriteSheet, int ID, int width = 95, int height = 95)
        {
            IntPtr[] NewTImages = new IntPtr[256];



            //if (SDL.SDL_QueryTexture(spriteSheet, out type, out access, out width, out height) != 0) 
            //{ 
            //    string GetSDLError([CallerLineNumber] int lineNum = 0) => "SDL Error in file TileLoader : " + SDL.SDL_GetError() + " at line " + lineNum; 
            //
            //    Client.FancyPrint(GetSDLError(), ConsoleColor.DarkRed); 
            //}

            for (uint i = 0; i < 256; i++)
            {

                IntPtr surface = SDL.SDL_CreateRGBSurface(0, width / 3, height / 3, 32, 0, 0, 0, 0);
                var rect = new SDL.SDL_Rect { x = width / 3, y = height / 3, w = width / 3, h = height / 3 };
                var destrect = new SDL.SDL_Rect { x = 0, y = 0, w = width / 3, h = height / 3 };

                SDL.SDL_BlitSurface(spriteSheet, ref rect, surface, ref destrect);
                if (((i >> 1) & 1) == 1)
                {
                    rect.y = 0;
                    rect.x = width / 3;
                    SDL.SDL_BlitSurface(spriteSheet, ref rect, surface, ref destrect);
                }
                if (((i >> 3) & 1) == 1)
                {
                    rect.y = height / 3;
                    rect.x = width * 2 / 3;
                    SDL.SDL_BlitSurface(spriteSheet, ref rect, surface, ref destrect);
                }
                if (((i >> 5) & 1) == 1)
                {
                    rect.y = height * 2 / 3;
                    rect.x = width / 3;
                    SDL.SDL_BlitSurface(spriteSheet, ref rect, surface, ref destrect);
                }
                if (((i >> 7) & 1) == 1)
                {
                    rect.y = height / 3;
                    rect.x = 0;
                    SDL.SDL_BlitSurface(spriteSheet, ref rect, surface, ref destrect);
                }


                if (((i >> 1) & 1) == 1 && ((i >> 7) & 1) == 1)
                {
                    rect.y = 0;
                    rect.x = 0;
                    SDL.SDL_BlitSurface(spriteSheet, ref rect, surface, ref destrect);
                }
                if (((i >> 1) & 1) == 1 && ((i >> 3) & 1) == 1)
                {
                    rect.y = 0;
                    rect.x = width * 2 / 3;
                    SDL.SDL_BlitSurface(spriteSheet, ref rect, surface, ref destrect);
                }
                if (((i >> 3) & 1) == 1 && ((i >> 5) & 1) == 1)
                {
                    rect.y = height * 2 / 3;
                    rect.x = width * 2 / 3;
                    SDL.SDL_BlitSurface(spriteSheet, ref rect, surface, ref destrect);
                }
                if (((i >> 5) & 1) == 1 && ((i >> 7) & 1) == 1)
                {
                    rect.y = height * 2 / 3;
                    rect.x = 0;
                    SDL.SDL_BlitSurface(spriteSheet, ref rect, surface, ref destrect);
                }


                //images[i] = SDL.SDL_CreateTextureFromSurface(renderer.SDLrenderer, surface);
                NewTImages[i] = SDL.SDL_CreateTextureFromSurface(renderer.SDLrenderer, surface);
            }



            FancyTiles.Add(ID, NewTImages);
        }
    }
}
