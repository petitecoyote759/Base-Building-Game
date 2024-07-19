using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#pragma warning disable // a ton of warnings about things not being defined, the json will define it.


namespace Base_Building_Game
{
    public static partial class General
    {
        public class WorldJson
        {
            public float PlayerX { get; set; }
            public float PlayerY { get; set; }
            public string Name { get; set; }
            public SectorJson[,] Sectors { get; set; }
            public int SectorX { get; set; }
            public int SectorY { get; set; }
            public string Research { get; set; }
        }

        public class SectorJson
        {
            public string MapData { get; set; }
            public string ExtraInfo { get; set; }
        }
    }
}
