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
            public Stack<Vector2>? path { get; set; } = new Stack<Vector2>();
            public WorkCamp camp { get; }
            Vector2[]? nextPositions = null;
            const float MovementScalar = 100;
            int t;
            public Men(Vector2 pos,WorkCamp camp)
            {
                this.pos = pos;
                this.camp = camp;
                LoadedActiveEntities.Add(this);
            }

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            public void Action(int dt)
            {

                /*if (targetedItem is null && path.Count == 0)
                {
                    if (heldItem is null)
                    {
                        FindItem();
                        
                    }
                    else
                    {
                        DepositItem();
                        ReturnToCamp();
                    }
                    return;
                }
                
                if (path is not null)
                {
                    if (path.Count != 0 || nextPositions is not null )
                    {         
                        if (nextPositions is null)
                        {
                            AddNextNodes();
                        }
                        if (nextPositions is not null)
                        {
                            t = t + dt;
                            if (t >= MovementScalar)
                            {
                                t = (int)MovementScalar;
                            }
                            this.pos = Bézier.ComputeBézier(t / MovementScalar, nextPositions);
                            if (t == MovementScalar)
                            {
                                nextPositions = null;
                                t = 0;
                                if (path.Count == 0)
                                {
                                    if (targetedItem is not null)
                                    {
                                        PickupItem();
                                    }
                                    else
                                    {
                                        DepositItem();
                                    }
                                }
                            }
                            return;
                        } 
                        
                    }   
                    return;
                }*/


                if (targetedItem is null && path.Count == 0 && nextPositions is null)
                {
                    if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastTimeToCalcPath > 10000)
                    {
                        if (heldItem is null)
                        {
                            FindItem();
                            if (path is null)
                            {
                                targetedItem.Targeted = false;
                                targetedItem = null;
                                path = new Stack<Vector2>();
                                lastTimeToCalcPath = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                            }
                        }
                        else
                        {
                            ReturnToCamp();
                        }
                       
                    }
                    return;
                }

                if (path.Count != 0 || nextPositions is not null)
                {
                    if (nextPositions is null)
                    {
                        AddNextNodes();
                    }
                    Move(dt);
                }


                
                

                if (nextPositions is null && path.Count == 0)
                {
                    if (heldItem is null)
                    {
                        PickupItem();
                    }
                    else
                    {
                        DepositItem();
                    }
                }





            }


            private static float RoughDist(Vector2 a, Vector2 b)
            {
                return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
            }

            const int MaxItemDist = 70;
            public void FindItem()
            {
                //Returns all of the items in the loaded entities that are not currently being targeted, ordered by distance from the entity.
                //This should be fixed soon, I dont like the issue that I brought up with M. 
                IEnumerable<Item> items =  (from item in LoadedEntities
                                where RoughDist(item.pos, pos) < MaxItemDist
                                where (item is Item item1 && !item1.Targeted)
                                orderby (item.pos - pos).LengthSquared() ascending
                                select (Item)item);
               
                //Whenever you want items to not be targeted, add the specification into this foreach loop. 
                foreach (Item item in items)
                {
                    item.Targeted = true;
                    targetedItem = item;
                    AStar pathing = new AStar(world.Walkable, item.pos, this.pos);
                    path = pathing.GetPath(50);

                    break;
                }
            }
            public bool PickupItem()
            {
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
                AStar pathing = new AStar(world.Walkable, camp.pos, this.pos);
                path = pathing.GetPath(50);
            }
            
            public void AddNextNodes()
            {
                int size = path.Count >= 3 ? 3 : path.Count + 1;
                nextPositions = new Vector2[size];
                nextPositions[0] = pos;
                for (int i = 1; i < size; i++)
                {
                    nextPositions[i] = path.Pop();
                }
            }
#pragma warning disable CS8604 // next positions could be null
            /// <summary>
            /// Dont call when next positions is null
            /// </summary>
            public void Move(int dt)
            {
                t += dt;
                if (t >= MovementScalar)
                {
                    t = (int)MovementScalar;
                }

                if ((pos - player.pos).LengthSquared() < (renderer.screenwidth * renderer.screenwidth) / (float)(renderer.zoom * renderer.zoom))
                {
                    this.pos = Bézier.ComputeBézier(t / MovementScalar, nextPositions);
                }
                else
                {
                    if (nextPositions.Length != 0)
                    {
                        this.pos = new Vector2(nextPositions[0].X + t * (nextPositions[1].X - nextPositions[0].X), nextPositions[0].Y + t * (nextPositions[1].Y - nextPositions[0].Y));
                    }
                }


                if (heldItem is not null)
                {
                    heldItem.pos = this.pos;
                }
                if (t == MovementScalar)
                {
                    nextPositions = null;
                    t = 0;
                }
            }
           
        }
    }
}
