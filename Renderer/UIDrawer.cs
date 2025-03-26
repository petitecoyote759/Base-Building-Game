using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;
using Short_Tools;
using static Short_Tools.General;
using static Base_Building_Game.General;
using SDL2;
using static SDL2.SDL;
using System.Numerics;



namespace Base_Building_Game
{
    public static partial class General
    {
        public partial class Renderer
        {
            public void DrawUI()
            {
                IVect MPos = getMousePos();


                if (player.turret is null)
                {
                    #region Selected Buildings
                    DrawBP(GetBlockx(MPos.x), GetBlocky(MPos.y), "MouseBox");

                    if (player.selectedTile is not null)
                    {
                        DrawBP(player.selectedTile.Value.x, player.selectedTile.Value.y, "SelectBox");
                    }

                    // MouseBox   return (zoom * x) - (zoom * player.camPos.x / 32) + halfscreenwidth;


                    if (HotbarSelected != -1)
                    {
                        short ID = hotbar[HotbarSelected];
                        if (ID != 0)
                        {
                            if (BuildingImages.ContainsKey(ID))
                            {
                                Building building = BuildingIDToBuilding((BuildingID)ID, new IVect(int.MinValue, int.MinValue), true);

                                DrawBP(
                                    GetBlockx(MPos.x),
                                    GetBlocky(MPos.y),
                                    BuildingImages[ID][Research[ID]],
                                    zoom * building.xSize,
                                    zoom * building.ySize,
                                    90d * player.CurrrentRotation);
                            }
                        }
                    }
                    #endregion Selected Buildings




                    #region Port UI
                    if (player.selectedTile is IVect pos)
                    {
                        if (world.GetTile(pos.x, pos.y).building is Building building)
                        {
                            if (building.ID == (short)BuildingID.SmallPort)
                            {
                                DrawBP(building.pos.X - 1, building.pos.Y - 2, "Port UI", zoom * 3, zoom * 2);
                            }
                            else if (building.ID == (short)BuildingID.MedPort)
                            {
                                DrawBP(building.pos.X - 2, building.pos.Y - 4, "Port UI", zoom * 6, zoom * 4);
                            }
                        }
                    }
                    #endregion Port UI
                }










                #region Hotbar
                // 356 x 38 def size, scaled upp to 712 x 76
                Draw((screenwidth / 2) - (712 / 2), screenheight - 76, 712, 76, "Hotbar");
                for (int i = 0; i < 10; i++)
                {
                    short ID = hotbar[i];
                    if (ID == 0) { continue; }

                    if (!BuildingImages.ContainsKey(ID)) { continue; }

                    Draw(halfscreenwidth - (712 / 2) + 70 * i + 4, screenheight - 70, 64, 64, BuildingImages[ID][Research[ID]]);
                }


                if (settings.Debugging)
                {
                    Write(0, 0, 50, 50, $"({(int)player.pos.X}, {(int)player.pos.Y})");
                    Write(0, 60, 50, 50, (player.SectorPos).ToString());
                }
                #endregion Hotbar






                #region Minimap, and Big Map
                if (!MapOpen)
                {
                    SDL_Rect srcrect = new SDL_Rect()
                    {
                        x = Math.Max(Math.Min((int)player.pos.X - screenwidth / 20, SectorSize - screenwidth / 10), 0),
                        y = Math.Max(Math.Min((int)player.pos.Y - screenwidth / 20, SectorSize - screenwidth / 10), 0),
                        w = screenwidth / 10,
                        h = screenwidth / 10
                    };

                    SDL_Rect dstrect = new SDL_Rect() { x = screenwidth * 9 / 10, y = 0, h = screenwidth / 10, w = screenwidth / 10 };

                    SDL_RenderCopy(SDLrenderer, images["Map"], ref srcrect, ref dstrect);


                    // draw player
                    Draw(screenwidth * 19 / 20, screenwidth * 1 / 20, screenwidth / 100, screenwidth / 100, "Player", player.angle);
                }


                if (MapOpen)
                {
                    // TODO: make an actual image for the border.
                    // Border
                    Draw(
                        (screenwidth * 1 / 4) - screenwidth / 100,
                        (screenheight - (screenwidth / 2)) / 2 - screenwidth / 100,
                        screenwidth / 2 + screenwidth / 50,
                        screenwidth / 2 + screenwidth / 50,
                        "MenuButton"
                        );


                    SDL_Rect srcrect = new SDL_Rect()
                    {
                        x = Math.Max(Math.Min((int)player.pos.X - screenwidth / 8, SectorSize - screenwidth / 4), 0),
                        y = Math.Max(Math.Min((int)player.pos.Y - screenwidth / 8, SectorSize - screenwidth / 4), 0),
                        w = screenwidth / 4,
                        h = screenwidth / 4
                    };

                    SDL_Rect dstrect = new SDL_Rect() { x = screenwidth * 1 / 4, y = (screenheight - screenwidth * 2 / 4) / 2, h = screenwidth * 2 / 4, w = screenwidth * 2 / 4 };

                    SDL_RenderCopy(SDLrenderer, images["Map"], ref srcrect, ref dstrect);


                    // draw player on bigger map
                    Draw(screenwidth * 99 / 200, screenheight * 99 / 200, screenwidth / 100, screenwidth / 100, "Player", player.angle);
                }



                #endregion Minimap and Big Map











                #region Boat and Boat Turrets
                foreach (Boat boat in (from entity in LoadedActiveEntities where entity is Boat select (Boat)entity).ToArray())
                {
                    Vector2 blockMousePos = new Vector2(GetFBlockx(MPos.x), GetFBlocky(MPos.y));

                    if (MathF.Abs(boat.pos.X - blockMousePos.X) < 0.5f &&
                        MathF.Abs(boat.pos.Y - blockMousePos.Y) < 0.5f)
                    {
                        Draw(GetPx(boat.pos.X) - zoom / 4, GetPy(boat.pos.Y) - zoom, zoom / 2, zoom / 2, "Interact");
                    }
                }

                foreach (Turret turret in (from entity in LoadedActiveEntities where entity is Turret select (Turret)entity).ToArray())
                {
                    Vector2 blockMousePos = new Vector2(GetFBlockx(MPos.x), GetFBlocky(MPos.y));

                    if (MathF.Abs(turret.pos.X - blockMousePos.X + 0.5f) < 0.5f &&
                        MathF.Abs(turret.pos.Y - blockMousePos.Y + 0.5f) < 0.5f)
                    {
                        Draw(GetPx(turret.pos.X + 0.5f) - zoom / 4, GetPy(turret.pos.Y + 0.5f) - zoom, zoom / 2, zoom / 2, "Interact");
                    }
                }
                #endregion Boat and Boat Turrets





                #region Commands

                if (TextBarOpen)
                {
                    Draw(
                        0,
                        screenheight * 9 / 10,
                        screenwidth / 2,
                        screenheight * 1 / 10,
                        "Text Bar"
                        );


                    SDL2.SDL_ttf.TTF_SizeText(Font, Handlers.CommandHandler.commandLine, out int w, out int h);

                    WriteLength(
                        posx: 0,
                        posy: screenheight * 9 / 10,
                        width: w,
                        height: screenheight * 1 / 20,
                        text: Handlers.CommandHandler.commandLine
                        );
                }


                #endregion
            }




            #region Get Block Positions
            public int GetBlockx(int x)
            {
                return (int)((x - halfscreenwidth + (zoom * player.camPos.X)) / zoom);
            }
            public int GetBlocky(int y)
            {
                return (int)((y - halfscreenheight + (zoom * player.camPos.Y)) / zoom);
            }


            public float GetFBlockx(int x)
            {
                return ((x - halfscreenwidth + (zoom * player.camPos.X)) / zoom);
            }
            public float GetFBlocky(int y)
            {
                return ((y - halfscreenheight + (zoom * player.camPos.Y)) / zoom);
            }
            #endregion Get Block Positions
        }
    }
}
