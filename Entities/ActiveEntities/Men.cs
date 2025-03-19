using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;
using Base_Building_Game.Entities.AStar;
using System.Drawing.Printing;


#pragma warning disable CS8602 // dereference of a possibly null reference. its to do with paths, again, i trust bayl on this



namespace Base_Building_Game
{
    public static partial class General
    {
        /// <summary>
        /// THe class representing the men.
        /// </summary>
        public class Men : IActiveEntity
        {
            public Vector2 pos { get; set; }
            public Item? heldItem {get; set; } = null;
            public Item? targetedItem { get; set; } = null;
            long lastTimeToCalcPath;
            public Queue<Vector2>? path { get; set; } = new Queue<Vector2>();
            public WorkCamp camp { get; }
            internal Vector2[]? nextPositions = null;
            const float MovementScalar = 100;
            const float speed = 0.005f;
            internal float currentSpeed = speed;
            int t;
            AStar pather = new AStar(Walkable, world.GetTileHeuristic, 50, true);
            static bool Walkable(int x, int y) => world.Walkable(x, y, false);
            public Men(Vector2 pos,WorkCamp camp)
            {
                this.pos = pos;
                this.camp = camp;
                LoadedActiveEntities.Add(this);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            public void Action(int dt)
            {
                if (RoughDist(pos, player.pos) > MenTeleportDistance) 
                {
                    HighDistanceAction(dt);
                    return;
                }


                if (highDistance)
                {
                    if (targetedItem is not null) { targetedItem.Targeted = false; }
                    highDistance = false;
                }




                if (path is not null && path.Count > 0)
                {
                    Move(dt);

                    if (heldItem is not null)
                    {
                        heldItem.pos = this.pos;
                        heldItem.InExtractor = false;
                    }

                    return;
                }




                if (heldItem is not null)
                {                 

                    if (pos == camp.pos)
                    {
                        DepositItem();
                    }
                    else
                    {
                        path = pather.GetPath(pos, camp.pos);
                        if (RoughDist(player.pos, pos) < MenBezierDistance)
                        {
                            path = Bezier.GetBezier(path, 0.2f);
                        }
                    }
                }
                else
                {
                    if (!PickupItem()) // try to pick up item, if i cant then find item
                    {
                        FindItem();
                    }
                }
            }


            private static float RoughDist(Vector2 a, Vector2 b)
            {
                return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
            }

            const int MaxItemDist = 70;
            public void FindItem(bool far = false)
            {
                //Returns all of the items in the loaded entities that are not currently being targeted, ordered by distance from the entity.
                //This should be fixed soon, I dont like the issue that I brought up with M.
                IEntity[] entities = LoadedEntities.ToArray();
                Item[] items =  (from item in entities
                                where item is not null
                                where RoughDist(item.pos, pos) < MaxItemDist
                                where (item is Item item1 && !item1.Targeted)
                                orderby (item.pos - pos).LengthSquared() ascending
                                select (Item)item).ToArray();

                //Whenever you want items to not be targeted, add the specification into this foreach loop. 
                foreach (Item item in items)
                {
                    item.Targeted = true;
                    targetedItem = item;
                    if (!far)
                    {
                        path = pather.GetPath(pos, targetedItem.pos);
                        if (RoughDist(player.pos, pos) < MenBezierDistance)
                        {
                            path = Bezier.GetBezier(path, 0.2f);
                        }
                    }

                    break;
                }
            }
            public bool PickupItem()
            {
                if (targetedItem is null) { return false; }

                if (targetedItem.pos == this.pos)
                {
                    heldItem = targetedItem;
                    targetedItem = null;
                    return true;
                }
                return false;
            }
#pragma warning disable CS8604 // heldItem could be null
            /// <summary>
            /// Do not call when heldItem is null ig.
            /// </summary>
            public bool DepositItem()
            {
                if ((IVect)camp.pos == (IVect)this.pos)
                {
                    LoadedEntities.Remove(heldItem);
                    heldItem = null;

                    return true;
                }
                return false;
            }
#pragma warning restore CS8604
            public void ReturnToCamp()
            {
                path = pather.GetPath(pos, camp.pos);
            }
            
            public void AddNextNodes()
            {
                int size = path.Count >= 3 ? 3 : path.Count + 1;
                nextPositions = new Vector2[size];
                nextPositions[0] = pos;
                for (int i = 1; i < size; i++)
                {
                    nextPositions[i] = path.Dequeue();
                }
            }
#pragma warning disable CS8604 // next positions could be null
            /// <summary>
            /// Dont call when next positions is null
            /// </summary>
            public void Move(int dt)
            {
                if (path is null || path.Count == 0) { return; }

                Vector2 nextNode = path.Peek();
                if (nextNode == pos) { path.Dequeue(); return; }

                if (world.GetTile(pos.X, pos.Y).building?.ID == (short)BuildingID.Path)
                {
                    currentSpeed = speed * PathSpeedMultiplier;
                }


                Vector2 dir = (nextNode - pos);

                Vector2 normDir = Vector2.Normalize(dir);

                Vector2 step = normDir * Math.Min(dir.Length(), currentSpeed * dt); // pretty sure this is wrong

                this.pos += step;
            }




            float chargedMovement = 0f;
            float requiredDistance = 0f;
            bool highDistance = false;
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            private void HighDistanceAction(int dt)
            {
                /*
                
                if targeting an item, charge up teleport.
                else, target an item

                */

                pos = camp.pos;
                if (heldItem is not null)
                {
                    LoadedEntities.Remove(heldItem);
                    heldItem.InExtractor = false;
                    targetedItem = null;
                    heldItem = null;
                }
                path = null;
                highDistance = true;

                if (targetedItem is not null)
                {
                    chargedMovement += speed * dt;
                    if (requiredDistance <= chargedMovement)
                    {
                        LoadedEntities.Remove(targetedItem);
                        targetedItem.InExtractor = false;
                        targetedItem = null;
                        // TODO: add the whole adding this to the damn thingie.
                        chargedMovement -= requiredDistance;
                    }
                }
                else
                {
                    FindItem(true);
                    if (targetedItem is not null)
                    {
                        requiredDistance = (targetedItem.pos - pos).Length();
                    }
                }
            }
        }
    }
}
