using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Printing;
using static Short_Tools.General;


namespace Base_Building_Game
{
    public static partial class General
    {
        public static Dictionary<int, IntPtr[]> FancyTiles = new Dictionary<int, IntPtr[]>()
        {

        };


        public static void LoadFancyTiles()
        {
            createImages(renderer.images["GrassSS"], General.images["GrassSS"], (int)TileID.Grass, 95, 95);
            createImages(renderer.images["SandSS"], General.images["SandSS"], (int)TileID.Sand, 96, 96);
            createImages(renderer.images["OceanSS"], General.images["OceanSS"], (int)TileID.Ocean, 96, 96);
        }



        public static unsafe void Test(string texture)
        {
            Print("Starting thing");

            Bitmap bitmap = new Bitmap(texture);

            Print(bitmap.Width);
            
            Print("Ending thing");
        }








        public static void createImages(IntPtr spriteSheet, string path, int ID, int width = 95, int height = 95)
        {
            IntPtr[] NewTImages = new IntPtr[256];



#pragma warning disable CA1416 // getting upset that this only works on windows later than 6.1.
            Bitmap bitmap = new Bitmap(path);

            width = bitmap.Width;
            height = bitmap.Height;
#pragma warning restore CA1416

            if (path == General.images["GrassSS"])
            {
                if (!TexturePackedImages["GrassSS"])
                {
                    width--; height--;
                }
            }




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
