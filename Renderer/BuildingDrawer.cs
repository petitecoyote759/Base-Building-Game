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
            public void DrawBuildings()
            {
                int zoom = renderer.zoom;
                float px = player.camPos.X;
                float py = player.camPos.Y;

                int Left = (int)((px) - (halfscreenwidth / zoom) - 4);
                int Top = (int)((py) - (halfscreenheight / zoom) - 4);




                for (int x = Left; x < Left + screenwidth / zoom + 8; x++)
                {
                    for (int y = Top; y < Top + screenheight / zoom + 8; y++)
                    {
                        Tile tile = world.GetTile(x, y);
                        lock (tile)
                        {
                            if (tile.building is null) { continue; }

                            if (BuildingImages.ContainsKey(tile.building.ID) && tile.building is not Linker)
                            {
                                if (tile.building is ConnectingBuilding CBuilding) { DrawConnectors(CBuilding); }


                                DrawBP(x, y,
                                    BuildingImages[tile.building.ID][Research[tile.building.ID]],
                                    zoom * tile.building.xSize,
                                    zoom * tile.building.ySize,
                                    tile.building.rotation * 90d
                                    );
                            }
                        }

                    }
                }
            }

















            public void DrawConnectors(ConnectingBuilding building)
            {
                if (building.Connections(world.GetTile(building.pos.x, building.pos.y - 1)))
                {
                    DrawBP(building.pos.x, building.pos.y, building.connectionImage, 0d);
                }

                if (building.Connections(world.GetTile(building.pos.x + 1, building.pos.y)))
                {
                    DrawBP(building.pos.x, building.pos.y, building.connectionImage, 90d);
                }

                if (building.Connections(world.GetTile(building.pos.x, building.pos.y + 1)))
                {
                    DrawBP(building.pos.x, building.pos.y, building.connectionImage, 180d);
                }

                if (building.Connections(world.GetTile(building.pos.x - 1, building.pos.y)))
                {
                    DrawBP(building.pos.x, building.pos.y, building.connectionImage, 270d);
                }
            }
        }
    }
}
