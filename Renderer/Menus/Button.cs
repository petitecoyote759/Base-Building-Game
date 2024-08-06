using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using IVect = Short_Tools.General.ShortIntVector2;
using static Short_Tools.General;


namespace Base_Building_Game
{
    public static partial class General
    {
        public partial class Renderer
        {
            public void DrawButton(int x, int y, int width, int height, string text)
            {
                renderer.Draw(x, y, width, height, renderer.images["MenuButton"]);

                IVect mpos = getMousePos();

                if (x <= mpos.x && x + width >= mpos.x &&
                    y <= mpos.y && y + height >= mpos.y)
                {
                    Draw(x, y, width, height, images["SelectedButton"]);
                }


                WriteOnFullWidth(x, y, width, height, text);
            }

            public void WriteOnFullWidth(int x, int y, int width, int height, string text, SDL2.SDL.SDL_Color? colour = null)
            {
                colour ??= Black;

                if (text.Length != 0)
                {
                    Write(x + width / 10, y, Math.Min(width / text.Length, height), height, text, colour);
                }
            }
        }
    }
}
