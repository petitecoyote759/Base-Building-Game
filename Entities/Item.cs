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

        public enum ItemID : short
        {
            Wood =1,
        }
        /// <summary>
        /// The class for item entities.
        /// </summary>
        public class Item : IEntity 
        {
            public ItemID ID;
            public IVect pos { get; set; }

            public Item(ItemID id,IVect pos)
            {
                ID = id;
                this.pos = pos;
                LoadedEntities.Add(this);
            }
        }
    }
}
