using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;


namespace Base_Building_Game
{
    public static partial class General
    {
        public static void Tick(int dt)
        {
            if (ActiveKeys["Mouse"])
            {
                if (InGame)
                {
                    if (HotbarSelected >= 0 && HotbarSelected < 10)
                    {
                        IVect pos = getMousePos();

                        hotbar.BuildBuilding(HotbarSelected, renderer.GetBlockx(pos.x), renderer.GetBlocky(pos.y));
                    }
                }
            }

            FBuilding[] buildings = FBuildings.ToArray();

            foreach (FBuilding building in buildings)
            {
                building.Action(dt);
            }
        }
    }
}
