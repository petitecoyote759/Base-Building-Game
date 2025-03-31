using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;
using static SDL2.SDL;
using static System.MathF;




namespace Base_Building_Game
{
    public static partial class General
    {
        public static void Tick(int dt)
        {
            Time = (Time + dt) % TimePerDay;





            if (mouseDown)
            {
                if (MenuState.IsInGame())
                {
                    if (HotbarSelected >= 0 && HotbarSelected < 10)
                    {
                        IVect pos = getMousePos();

                        hotbar.BuildBuilding((BuildingID)hotbar[HotbarSelected], renderer.GetBlockx(pos.x), renderer.GetBlocky(pos.y));
                    }
                    else
                    {
                        IVect pos = getMousePos();

                        player.selectedTile = new IVect(renderer.GetBlockx(pos.x), renderer.GetBlocky(pos.y));
                    }
                }
            }


#pragma warning disable CS8602 // dereference of a possibly null reference -> active sector aint gonna be null
#pragma warning disable CS8600 // same thing
            if (ActiveKeys[SDL2.SDL.SDL_Keycode.SDLK_BACKSPACE])
            {
                if (MenuState.IsInGame())
                {
                    if (player.selectedTile is not null)
                    {
                        int x = player.selectedTile.Value.x;
                        int y = player.selectedTile.Value.y;
                        if ((ActiveSector[x,y].building is not null && (ActiveSector[x,y].building.GetType() == typeof(Linker))))
                        {
                            Linker selectedLinker = (Linker)ActiveSector[x,y].building;
                            IVect topLeft = selectedLinker.connectedBuilding.pos;
                            Linker.ClearLinkers(topLeft,selectedLinker.connectedBuilding.xSize,selectedLinker.connectedBuilding.ySize);     
                        }
                        else if (ActiveSector[x,y].building is not null && (ActiveSector[x, y].building.xSize >= 1 || ActiveSector[x, y].building.ySize >= 1))
                        {
                            Linker.ClearLinkers(new IVect(player.selectedTile.Value.x, player.selectedTile.Value.y), ActiveSector[x, y].building.xSize, ActiveSector[x, y].building.ySize);
                        }
                        else
                        {
                            ActiveSector[x, y].building = null; // TODO: Add refund
                        }

                    }
                }
            }
#pragma warning restore CS8602
#pragma warning restore CS8600







            FBuilding[] buildings = FBuildings.ToArray();

            foreach (FBuilding building in buildings)
            {
                building.Action(dt);
            }
        }
    }
}
