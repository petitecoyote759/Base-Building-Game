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
            private Dictionary<short, short> Items = new Dictionary<short, short>();

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
            private Dictionary<short, short> Empty()
            {
                Dictionary<short, short> EmptyItems = new Dictionary<short, short>();

                foreach (short value in Enum.GetValues(typeof(ItemID))) { EmptyItems.Add(value, 0); }
                // loops through enum, adds { value, 0 }

                return EmptyItems;
            }



            public bool AddItem(ItemID item)
            {
                return AddItem((short)item);
            }
            public bool AddItem(short item)
            {
                Items[item]++;
                return true; // TODO: add capacity to inventories.
            }






            public static Inventory FromString(string data)
            {
                Inventory inventory = new Inventory();

                Array enums = Enum.GetValues(typeof(ItemID));

                for (int i = 0; i < data.Length; i++)
                {
                    char value = data[i];
                    inventory.Items[(short)(enums.GetValue(i) ?? ItemID.Error)] = (short)(((short)value) & 0b01111111_11111111);
                }
                return inventory;
            }




            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();

                foreach (KeyValuePair<short, short> pair in Items)
                {
                    builder.Append($"{Enum.GetName(typeof(ItemID), pair.Key)}: {(int)pair.Value}");
                    if (pair.Key != Items.Last().Key) { builder.Append(", "); }
                }

                return builder.ToString();
            }



            public string ToDataString()
            {
                StringBuilder builder = new StringBuilder();

                foreach (KeyValuePair<short, short> pair in Items)
                {
                    char value = (char)(((pair.Value & 0b01111111_11111111) | 0b10000000_00000000)); 
                    // needs to have some value unfortuanely, so ima have to have the biggest bit gone.
                    builder.Append(value);
                }


                return builder.ToString();
            }
        }
    }
}
