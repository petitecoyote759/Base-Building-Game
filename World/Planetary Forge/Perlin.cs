using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;



namespace Base_Building_Game
{
    public static partial class General
    {
        public class PerlinMap
        {
            public const int PerlinWidth = 32;

            public IVect[,] vectors = new IVect[SectorSize / PerlinWidth + 1, SectorSize / PerlinWidth + 1];

            public IVect this[int x, int y] => vectors[x, y];
            public IVect this[IVect pos] => vectors[pos.x, pos.y];

            public static readonly IVect[] VectorPosibilities = new IVect[] { 
                new IVect(1,1), new IVect(-1,1),
                new IVect(-1,-1), new IVect(1,-1)
            };



            public PerlinMap()
            {
                GenVectors();
            }




            public void GenVectors()
            {
                for (int x = 0; x < SectorSize / PerlinWidth + 1; x++)
                {
                    for (int y = 0; y < SectorSize / PerlinWidth + 1; y++)
                    {
                        vectors[x, y] = VectorPosibilities[randy.Next(0, 4)];
                    }
                }
            }



            public float GetValue(IVect pos)
            {
                return GetValue(pos.x, pos.y);
            }
            public float GetValue(int x, int y)
            {

                int Squarex = x / PerlinWidth;
                int Squarey = y / PerlinWidth;

                float northwest = Vector2.Dot(vectors[x / PerlinWidth, y / PerlinWidth], 
                    new Vector2((Squarex * PerlinWidth) - x, Squarey * PerlinWidth - y)) + 0.5f;

                float northeast = Vector2.Dot(vectors[(x) / PerlinWidth + 1, y / PerlinWidth],
                    new Vector2(((Squarex + 1) * PerlinWidth) - x, Squarey * PerlinWidth - y)) + 0.5f;

                float southwest = Vector2.Dot(vectors[x / PerlinWidth, (y) / PerlinWidth + 1],
                    new Vector2((Squarex * PerlinWidth) - x, ((Squarey + 1) * PerlinWidth) - y)) + 0.5f;

                float southeast = Vector2.Dot(vectors[(x) / PerlinWidth + 1, (y) / PerlinWidth + 1],
                    new Vector2(((Squarex + 1) * PerlinWidth) - x, ((Squarey + 1) * PerlinWidth) - y)) + 0.5f;

                float North = Lerp(northwest, northeast, ((x - Squarex * PerlinWidth) / (float)PerlinWidth));
                float South = Lerp(southwest, southeast ,((x - Squarex * PerlinWidth) / (float)PerlinWidth));

                return Lerp(North, South, ((y - Squarey * PerlinWidth) / (float)PerlinWidth));
            }



            public float Lerp(float a0, float a1, float w) // perc  is distance from A -> B
            {
                //return (a1 - a0) * w + a0;
                return (a1 - a0) * (3.0f - w * 2.0f) * w * w + a0;
                //return (a1 - a0) * ((w * (w * 6.0f - 15.0f) + 10.0f) * w * w * w) + a0;
            }
        }
    }
}
