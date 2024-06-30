using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;
using Short_Tools;
using static Short_Tools.General;
using static Base_Building_Game.General;


namespace Base_Building_Game
{
    public static partial class General
    {
        public partial class Renderer
        {
            public void DrawUI()
            {
                IVect MPos = getMousePos();
                DrawBP(GetBlockx(MPos.x), GetBlocky(MPos.y), "MouseBox");
                // MouseBox   return (zoom * x) - (zoom * player.camPos.x / 32) + halfscreenwidth;


                if (HotbarSelected != -1)
                {
                    short ID = hotbar[HotbarSelected];
                    if (ID != 0)
                    {
                        if (BuildingImages.ContainsKey(ID))
                        {
                            DrawBP(GetBlockx(MPos.x), GetBlocky(MPos.y), BuildingImages[ID][Research[ID]]);
                        }
                    }
                }



                // 356 x 38 def size, scaled upp to 712 x 76
                Draw((screenwidth / 2) - (712 / 2), screenheight - 76, 712, 76, "Hotbar");
                for (int i = 0; i < 10; i++)
                {
                    short ID = hotbar[i];
                    if (ID == 0) { continue; }

                    if (!BuildingImages.ContainsKey(ID)) { continue; }

                    Draw(halfscreenwidth - (712 / 2) + 70 * i + 4, screenheight - 70, 64, 64, BuildingImages[ID][Research[ID]]);
                }


                if (settings.Debugging)
                {
                    Write(0, 0, 50, 50, (player.pos / 32).ToString());
                    Write(0, 60, 50, 50, (player.SectorPos / 32).ToString());
                }
            }





            public int GetBlockx(int x)
            {
                return (x - halfscreenwidth + (zoom * player.camPos.x / 32)) / zoom;
            }
            public int GetBlocky(int y)
            {
                return (y - halfscreenheight + (zoom * player.camPos.y / 32)) / zoom;
            }
        }
    }
}
