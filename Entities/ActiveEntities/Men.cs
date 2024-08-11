using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection;
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
                   if (heldItem is null)
                   {
                        FindItem();
                   }
                   else
                   {
                        ReturnToCamp();
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
            public void FindItem()
            {
                //Returns all of the items in the loaded entities that are not currently being targeted, ordered by distance from the entity.
                //This should be fixed soon, I dont like the issue that I brought up with Maddie. 
                IEntity[] items =  (from item in LoadedEntities
                                where (item is Item && !((Item)item).Targeted)
                                orderby Vector2.Dot(item.pos - pos, item.pos - pos) ascending
                                select item).ToArray();
               
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
            public void Move(int dt)
            {
                t = t + dt;
                if (t >= MovementScalar)
                {
                    t = (int)MovementScalar;
                }
                this.pos = Bézier.ComputeBézier(t / MovementScalar, nextPositions);
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
