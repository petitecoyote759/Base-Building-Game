using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        public class Inventory
        {
            //                  ID   Count
            private Dictionary<short, int> Items = new Dictionary<short, int>();

            public int this[ItemID index] => Items[(short)index];
            public int this[short index] => Items[index];



            public Inventory()
            {
                Items = Empty();
            }




            /// <summary>
            /// makes a new dictionary, with all entries corresponding to the ItemID enum, and with a default
            /// value of 0.
            /// </summary>
            /// <returns></returns>
            private Dictionary<short, int> Empty()
            {
                Dictionary<short, int> EmptyItems = new Dictionary<short, int>();

                foreach (short value in Enum.GetValues(typeof(ItemID))) { Items.Add(value, 0); }
                // loops through enum, adds { value, 0 }

                return EmptyItems;
            }
        }
    }
}
