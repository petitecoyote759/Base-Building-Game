using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Drawing.Drawing2D;


namespace Base_Building_Game
{
    public static partial class General
    {

        public static class Bézier
        {
            public static Vector2 ComputeBézier(float t, params Vector2[] nodes)
            {
               
                switch (nodes.Length)
                {
                    case (2):
                        return new Vector2(nodes[0].X + t * (nodes[1].X - nodes[0].X), nodes[0].Y + t * (nodes[1].Y - nodes[0].Y));
                    case (3):
                        return new Vector2(nodes[1].X + (MathF.Pow(1 - t, 2)) * (nodes[0].X - nodes[1].X) + MathF.Pow(t, 2) * (nodes[2].X - nodes[1].X), nodes[1].Y + (MathF.Pow(1 - t, 2)) * (nodes[0].Y - nodes[1].Y) + MathF.Pow(t, 2) * (nodes[2].Y - nodes[1].Y));
                    case (4):
                        return new Vector2(MathF.Pow(1 - t, 3) * nodes[0].X + 3 * MathF.Pow(1 - t, 2) * t * nodes[1].X + 3 * (1 - t) * MathF.Pow(t, 2) * nodes[2].X + MathF.Pow(t, 3) * nodes[3].X, MathF.Pow(1 - t, 3) * nodes[0].Y + 3 * MathF.Pow(1 - t, 2) * t * nodes[1].Y + 3 * (1 - t) * MathF.Pow(t, 2) * nodes[2].Y + MathF.Pow(t, 3) * nodes[3].Y);
                    default:
                        throw new NotImplementedException("i am not doing higher than cubic");
                }
            }






        }

       
    }
}
