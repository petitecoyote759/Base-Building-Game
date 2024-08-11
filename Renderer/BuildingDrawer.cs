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
#pragma warning disable CS8618 // must have non null value -> its defined every time before use silly
            static Tile tempTile;
#pragma warning restore CS8618


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
                        tempTile = world.GetTile(x, y);
                        lock (tempTile)
                        {
                            if (tempTile.building is null) { continue; }

                            if (BuildingImages.ContainsKey(tempTile.building.ID) && tempTile.building is not Linker)
                            {
                                if (tempTile.building is ConnectingBuilding CBuilding) { DrawConnectors(CBuilding); }


                                DrawBP(x, y,
                                    BuildingImages[tempTile.building.ID][Research[tempTile.building.ID]],
                                    zoom * tempTile.building.xSize,
                                    zoom * tempTile.building.ySize,
                                    tempTile.building.rotation * 90d
                                    );
                            }
                        }

                    }
                }
            }

















            public void DrawConnectors(ConnectingBuilding building)
            {
                if (building.Connections(world.GetTile(building.pos.X, building.pos.Y - 1)))
                {
                    DrawBP(building.pos.X, building.pos.Y, building.connectionImage, 0d);
                }

                if (building.Connections(world.GetTile(building.pos.X + 1, building.pos.Y)))
                {
                    DrawBP(building.pos.X, building.pos.Y, building.connectionImage, 90d);
                }

                if (building.Connections(world.GetTile(building.pos.X, building.pos.Y + 1)))
                {
                    DrawBP(building.pos.X, building.pos.Y, building.connectionImage, 180d);
                }

                if (building.Connections(world.GetTile(building.pos.X - 1, building.pos.Y)))
                {
                    DrawBP(building.pos.X, building.pos.Y, building.connectionImage, 270d);
                }
            }
        }
    }
}
