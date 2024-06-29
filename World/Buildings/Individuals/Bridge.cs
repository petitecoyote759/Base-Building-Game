using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools
using static Short_Tools.General;




namespace Base_Building_Game
{
    public static partial class General
    {
        public class Bridge : Building
        {
            public short ID { get; set; }
            public Inventory? inventory { get; set; } 
            public int CurrentHealth { get; set; }
        }
    }
}
