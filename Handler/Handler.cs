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



#pragma warning disable CS8602 // defreference of a possible null reference -> silly, ik that small ports have inventories




namespace Base_Building_Game
{
    public static partial class General
    {
        static Dictionary<string, bool> ActiveKeys = new Dictionary<string, bool>()
        {
            { "w", false },
            { "a", false },
            { "s", false },
            { "d", false },
            { "Mouse", false },
            { "BACKSPACE", false },
        }; // the keys currently being pressed





        public class Handler : ShortHandler
        {

            public Handler() : base() { } // stick in base(Flag.Debug) to see what buttons are pressed 

            public override void Handle(string inp, bool down)
            {
                if (ActiveKeys.ContainsKey(inp)) { ActiveKeys[inp] = down; }


                if (!MenuState.IsInGame() && down)
                {
                    HandleMenus(inp);
                    return;
                }


                if (int.TryParse(inp, out int result)) // if it is a number
                {
                    HotbarSelected = result - 1;
                    if (result == 0)
                    {
                        HotbarSelected = 9;
                    }
                }

                switch (inp)
                {
                    case "w":

                        if (down && InGame && player.boat is not null)
                        {
                            player.boat.ThrustActive = true;
                        }
                        break;

                    case "s":

                        if (down && InGame && player.boat is not null)
                        {
                            player.boat.ThrustActive = false;
                        }
                        break;





                    case "ESCAPE":

                        if (down)
                        {
                            HotbarSelected = -1;
                            player.selectedTile = null;
                        }

                        break;




                    case "MouseWheel":

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

                        break;

                    case "F1":

                        if (down)
                        {
                            settings.Debugging = !settings.Debugging;
                        }

                        break;





                    case "b":

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


                DoBoatHandles(inp, down);
            }
        }
    }
}
