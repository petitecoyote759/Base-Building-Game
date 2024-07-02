using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;

namespace Base_Building_Game
{
    public static partial class General
    {
        /// <summary>
        /// THe class representing the men.
        /// </summary>
        public class Men : IActiveEntity
        {
            public IVect pos { get; set; }
            public Item? heldItem {get; set; } = null;
            public Item? targetedItem { get; set; } = null;
            public Stack<IVect>? path { get; set; } = null;
            public Men(IVect pos)
            {
                this.pos = pos;
                LoadedActiveEntities.Add(this);
            }

            public void Action(int dt)
            {
                if (targetedItem is null)
                {
                    FindItem();
                    return;
                }
                if (path is not null)
                {
                    if (path.Count != 0)
                    {
                        //TODO: Add bezier curves to smoothen out movement over a period of time.
                        this.pos = path.Pop() * 32;
                        return;
                    }
                    if (PickupItem())
                    {
                        return;
                        //TODO: Make the men return their camp.
                    }
                    //If this occurs, this is a pretty big issue. The men have followed their path to its conclusion, and their isnt an item there.
                    debugger.AddLog("Man failed to pick up an item once its path had concluded");

                }
                
            }
            public void FindItem()
            {
                //Returns all of the items in the loaded entities that are not currently being targeted, ordered by distance from the entity.
                //This should be fixed soon, I dont like the issue that I brought up with Maddie. 
                IEntity[] items =  (from item in LoadedEntities
                                where (item is Item && !((Item)item).Targeted)
                                orderby (item.pos - this.pos).MagSquared() ascending
                                select item).ToArray();
               
                //Whenever you want items to not be targeted, add the specification into this foreach loop. 
                foreach (Item item in items)
                {
                    item.Targeted = true;
                    targetedItem = item;
                    AStar pathing = new AStar(world.Walkable, item.pos, this.pos);
                    path = pathing.GetPath(100);
                    break;
                }
            }
            public bool PickupItem()
            {
                if (targetedItem.pos / 32 == this.pos)
                {
                    heldItem = targetedItem;
                    targetedItem = null;
                    LoadedEntities.Remove(targetedItem);
                    return true;
                }
                return false;
            }
            public void PathToItem()
            {
                throw new NotImplementedException();
            }
        }
    }
}
