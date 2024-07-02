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
            Wood = 1,
            Stone,
            Iron,
            Diamond
        }



        /// <summary>
        /// The class for item entities.
        /// </summary>
        public class Item : IEntity 
        {
            public short ID;
            public IVect pos { get; set; }
            public bool Targeted { get; set; }
            public bool InExtractor = false;

            public Item(short id,IVect pos)
            {
                ID = id;
                this.pos = pos;
                LoadedEntities.Add(this);
            }
        }




        static Dictionary<short, short> TileIDToItem = new Dictionary<short, short>()
        {
            { (short)TileID.Wood,    (short)ItemID.Wood },
            { (short)TileID.Stone,   (short)ItemID.Stone },
            { (short)TileID.Iron,    (short)ItemID.Iron },
            { (short)TileID.Diamond, (short)ItemID.Diamond },
        };
    }
}
