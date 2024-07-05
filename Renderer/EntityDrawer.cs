using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
            public void DrawEntities()
            {
                int zoom = renderer.zoom;
                float px = player.camPos.X;
                float py = player.camPos.Y;



                IEntity[] entities = LoadedEntities.ToArray();
                //This returns all of the entities which are on the screen using LINQ. It orders them by distance from the player in order to give them rendering priority if there are too many entities.
                IEntity[] entitiesToRender =
                    (from entity in entities
                     where GetPx(entity.pos.X) >= -zoom && GetPx(entity.pos.X) <= screenwidth && GetPy(entity.pos.Y) >= -zoom && GetPy(entity.pos.Y) <= screenheight
                     orderby Vector2.Dot(player.pos - entity.pos, player.pos - entity.pos) ascending
                     select entity).ToArray();

                IActiveEntity[] activeEntities = LoadedActiveEntities.ToArray();
                //This returns all of the entities which are on the screen using LINQ. It orders them by distance from the player in order to give them rendering priority if there are too many entities.
                IEntity[] activeEntitiesToRender =
                    (from entity in activeEntities
                     where GetPx(entity.pos.X) >= -zoom && GetPx(entity.pos.X) <= screenwidth && GetPy(entity.pos.Y) >= -zoom && GetPy(entity.pos.Y) <= screenheight
                     orderby Vector2.Dot(player.pos - entity.pos, player.pos - entity.pos) ascending
                     select entity).ToArray();


                //Iterating through all of the onscreen entities.
                foreach (IEntity entity in entitiesToRender)
                {
                    //If its an item, then we render using the itemImages dictionary.
                    if (entity is Item)
                    {

                        Item item = (Item)entity;
                        //DrawBP(entity.pos.x / 32, entity.pos.y / 32, ItemImages[(short)item.ID]);
                        DrawBP(entity.pos.X, entity.pos.Y, ItemImages[item.ID]);
                    }
                }




                foreach (IActiveEntity activeEntity in activeEntitiesToRender)
                {
                    if (activeEntity is Men)
                    {
                        DrawPP(activeEntity.pos.X, activeEntity.pos.Y, images["Man"]);
                    }
                    //When new entities are added, add them here:
                    //
                    if (activeEntity is Boat boat)
                    {
                        DrawBP(
                            boat.pos.X - (boat.Width / 2f),
                            boat.pos.Y - (boat.Length / 2f),
                            BoatImages[boat.ID][BoatResearch[boat.ID]],
                            zoom * boat.Width,
                            zoom * boat.Length,
                            boat.angle);

                        DrawBP(boat.pos.X - 0.1f, boat.pos.Y - 0.1f, "Short Studios Logo", zoom / 10, zoom / 10);
                    }
                }



                foreach (IActiveEntity activeEntity in activeEntitiesToRender)
                {
                    if (activeEntity is Turret turret)
                    {
                        DrawBP(activeEntity.pos.X, activeEntity.pos.Y, "Turret2", turret.angle);
                    }
                    if (activeEntity is Projectile bullet)
                    {
                        DrawBP(activeEntity.pos.X - 0.1f, activeEntity.pos.Y - 0.25f, "Projectile2", zoom / 4, zoom / 4, bullet.angle);
                    }
                }
            }
        }
    }
}
