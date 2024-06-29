using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        public partial class Renderer
        {
            public void DrawUI()
            {
                Write(0, 0, 50, 50, (player.pos / 32).ToString());
            }
        }
    }
}
