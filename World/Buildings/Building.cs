using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game.World
{
    public static partial class General
    {
        public enum BuildingID : short
        {
            None
        }



        public interface Building
        {
            public short ID { get; set; }

        }





        public interface FBuilding : Building
        {
            public void Action(long dt);
        }
    }
}
