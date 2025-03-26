using Short_Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;
using SDL2;
using static SDL2.SDL;


#pragma warning disable CS8602 // defreference of a possible null reference -> silly, ik that small ports have inventories




namespace Base_Building_Game
{
    public static partial class General
    {
        internal static Dictionary<SDL_Keycode, bool> ActiveKeys = new Dictionary<SDL_Keycode, bool>()
        {
            { SDL_Keycode.SDLK_w, false },
            { SDL_Keycode.SDLK_a, false },
            { SDL_Keycode.SDLK_s, false },
            { SDL_Keycode.SDLK_d, false },
            { SDL_Keycode.SDLK_BACKSPACE, false },
            { SDL_Keycode.SDLK_LSHIFT, false }
        }; // the keys currently being pressed

        static bool mouseDown = false;





        public class Handler : XXShortHandler
        {

            public Handler() : base() { } // stick in base(Flag.Debug) to see what buttons are pressed 






            internal override void HandleMousePress(bool down, bool mouseWheel = false)
            {
                if (mouseWheel)
                {
                    if (HotbarSelected == -1)
                    {
                        if (down)
                        {
                            renderer.zoom = renderer.zoom * 11 / 10;
                            if (renderer.zoom > 200) { renderer.zoom = 200; }
                        }
                        else
                        {
                            renderer.zoom = renderer.zoom * 9 / 10;
                            if (renderer.zoom < 10) { renderer.zoom = 10; }
                        }
                    }
                    else
                    {
                        player.CurrrentRotation = (player.CurrrentRotation + (down ? 1 : -1)) % 4;
                    }
                }
                else
                {
                    mouseDown = down;
                    HandleMenus("Mouse");
                }
            }



            static readonly Dictionary<SDL_Keycode, int> SDLKeyNumbers = new Dictionary<SDL_Keycode, int>
            {
                { SDL_Keycode.SDLK_0, 0},
                { SDL_Keycode.SDLK_1, 1},
                { SDL_Keycode.SDLK_2, 2},
                { SDL_Keycode.SDLK_3, 3},
                { SDL_Keycode.SDLK_4, 4},
                { SDL_Keycode.SDLK_5, 5},
                { SDL_Keycode.SDLK_6, 6},
                { SDL_Keycode.SDLK_7, 7},
                { SDL_Keycode.SDLK_8, 8},
                { SDL_Keycode.SDLK_9, 9},
            };



            // TODO: Fix this abomination
            public override void Handle(SDL_Keycode inp, bool down)
            {
                if (MenuState.IsInGame() && TextBarOpen && down && inp != SDL_Keycode.SDLK_ESCAPE)
                {
                    Handlers.CommandHandler.HandleCommands(inp);
                    return;
                }


                if (ActiveKeys.ContainsKey(inp)) { ActiveKeys[inp] = down; }

                if (MenuState == MenuStates.StartScreen && down && inp == SDL_Keycode.SDLK_F9)
                {
                    debugger.AddLog("Activating dev create world", ShortDebugger.Priority.INFO);
                    WorldGen.General.ReqCreateWorld();
                }

                if (!MenuState.IsInGame() && down)
                {
                    HandleMenus(inp.ToString().Substring(5)); // would like to fix this but for now it works
                    return;
                }



                if (SDLKeyNumbers.ContainsKey(inp) && down) // if it is a number
                {
                    int result = SDLKeyNumbers[inp];
                    HotbarSelected = result - 1;
                    if (result == 0)
                    {
                        HotbarSelected = 9;
                    }
                }

                switch (inp)
                {
                    case SDL_Keycode.SDLK_w:

                        if (down && MenuState.IsInGame() && player.boat is not null)
                        {
                            player.boat.ThrustActive = true;
                        }
                        break;

                    case SDL_Keycode.SDLK_s:

                        if (down && MenuState.IsInGame() && player.boat is not null)
                        {
                            player.boat.ThrustActive = false;
                        }
                        break;

                    case SDL_Keycode.SDLK_m:

                        if (down)
                        {
                            MapOpen = !MapOpen;
                        }

                        break;

                    case SDL_Keycode.SDLK_SLASH:

                        if (down && !TextBarOpen)
                        {
                            TextBarOpen = true;
                            Handlers.CommandHandler.commandLine = "/";
                        }

                        break;



                    case SDL_Keycode.SDLK_ESCAPE:

                        if (down)
                        {
                            TextBarOpen = false;
                            HotbarSelected = -1;
                            player.selectedTile = null;
                        }

                        break;

                    case SDL_Keycode.SDLK_F1:

                        if (down)
                        {
                            settings.Debugging = !settings.Debugging;
                        }

                        break;





                    case SDL_Keycode.SDLK_b:

                        if (!down) { break; }

                        if (player.selectedTile is IVect pos)
                        {
                            if (world.GetTile(pos.x, pos.y).building is Building building)
                            {
                                switch (building.ID)
                                {
                                    case (short)BuildingID.SmallPort:


                                        Skiff skiff = new Skiff();


                                        bool HasResources = true;
                                        foreach (var pair in skiff.ResourceCosts)
                                        {
                                            if (building.inventory[pair.Key] < pair.Value)
                                            {
                                                HasResources = false;
                                                break; // TODO: add feature init
                                            }
                                        }
                                        if (!HasResources) { break; }

                                        skiff = new Skiff(building.pos);
                                        skiff.pos = new Vector2(skiff.pos.X - 0.5f, skiff.pos.Y + 0.5f);
                                        LoadedActiveEntities.Add(skiff);
                                        

                                        break;







                                    case (short)BuildingID.MedPort:

                                        Destroyer destroyer = new Destroyer();


                                        HasResources = true;
                                        foreach (var pair in destroyer.ResourceCosts)
                                        {
                                            if (building.inventory[pair.Key] < pair.Value)
                                            {
                                                HasResources = false;
                                                break; // TODO: add feature init
                                            }
                                        }
                                        if (!HasResources) { break; }

                                        destroyer = new Destroyer(building.pos);
                                        destroyer.pos = new Vector2(destroyer.pos.X - 0.5f, destroyer.pos.Y + 0.5f);
                                        LoadedActiveEntities.Add(destroyer);

                                        break;




                                    case (short)BuildingID.LargePort:


                                        Battleship battleship = new Battleship();


                                        HasResources = true;
                                        foreach (var pair in battleship.ResourceCosts)
                                        {
                                            if (building.inventory[pair.Key] < pair.Value)
                                            {
                                                HasResources = false;
                                                break; // TODO: add feature init
                                            }
                                        }
                                        if (!HasResources) { break; }

                                        battleship = new Battleship(building.pos);
                                        battleship.pos = new Vector2(battleship.pos.X - 0.5f, battleship.pos.Y + 0.5f);
                                        LoadedActiveEntities.Add(battleship);

                                        break;
                                }
                            }
                        }

                        break;
                    
                }


                DoBoatHandles(inp.ToString().Substring(5), down);
            }
        }
    }
}
