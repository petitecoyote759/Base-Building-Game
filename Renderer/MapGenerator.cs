using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;
using Priority = Short_Tools.ShortDebugger.Priority;
using static SDL2.SDL;
using System.Drawing;


namespace Base_Building_Game
{
    public static partial class General
    {








        public static void ReqSaveMapImage(string path)
        {
            while (SavingMapImage) { Thread.Sleep(5); }

            renderer.TempMapPath = path;
            SavingMapImage = true;
        }


        /// <summary>
        /// DO NOT CALL THIS ON THE MAIN THREAD, CALL ReqSaveMapImage
        /// </summary>
        /// <param name="filePath"></param>
        public static void SaveMapImage(string filePath)
        {
            IntPtr surface = SDL_CreateRGBSurfaceWithFormat(0, SectorSize, SectorSize, 32, SDL.SDL_PIXELFORMAT_RGBA8888);


            SDL_LockSurface(surface);


            SDL_Rect srcrect = new SDL_Rect() { x = 0, y = 0, w = 32, h = 32 };
            SDL_Rect dstRect = new SDL_Rect { x = 0, y = 0, w = 1, h = 1 };


            for (int x = 0; x < SectorSize; x++) 
            {
                for (int y = 0; y < SectorSize; y++)
                {
                    dstRect.x = x; dstRect.y = y;


                    Tile tile = world.GetTile(x, y);

                    if (renderer.TileImages.ContainsKey(tile.ID))
                    {
                        GetTileColour(tile.ID, out uint colour);
                        GetBuildingColour(tile.building, ref colour);
                        SDL.SDL_FillRect(surface, ref dstRect, colour);
                        //SDL_BlitSurface(renderer.TileImages[tile.ID], ref srcrect, surface, ref dstRect);
                    }
                }
            }


            SDL.SDL_UnlockSurface(surface);

            SDL_image.IMG_SavePNG(surface, filePath);
            renderer.images["Map"] = SDL.SDL_CreateTextureFromSurface(renderer.SDLrenderer, surface);


            SDL.SDL_FreeSurface(surface);
        }



        static void GetTileColour(short ID, out uint colour)
        {
            switch (ID)
            {
                case (short)TileID.Grass:
                    colour = 0x00FF00FF; break;

                case (short)TileID.Sand:
                    colour = 0xFFFF00FF; break;

                case (short)TileID.Ocean:
                    colour = 0x3030FFFF; break;

                case (short)TileID.DeepOcean:
                    colour = 0x0000AAFF; break;

                case (short)TileID.Diamond:
                    colour = 0x0099FFFF; break;

                case (short)TileID.Iron:
                    colour = 0xFFAA00FF; break;

                case (short)TileID.Stone:
                    colour = 0x999999FF; break;

                case (short)TileID.Wood:
                    colour = 0x704000FF; break;


                default:
                    colour = 0xFFFFFFFF; break;
            }
        }
        static void GetBuildingColour(Building? building, ref uint colour)
        {
            if (building is null) { return; }

            switch (building.ID)
            {
                case (short)BuildingID.Wall:

                    // TODO: check if freind, if not then change it to be mildly more red
                    colour = 0xAAAAAA; break;
            }
        }
    }
}
