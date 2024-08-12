using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;



namespace Base_Building_Game
{
    public static partial class General
    {
        public class PerlinMap
        {
            public int PerlinWidth;

            public Vector2[,] vectors;

            #region this[]
            public IVect this[int x, int y] => vectors[x, y];
            public IVect this[IVect pos] => vectors[pos.x, pos.y];
            #endregion this[]


            public static readonly IVect[] VectorPosibilities = new IVect[] { 
                new IVect(1,1), new IVect(-1,1),
                new IVect(-1,-1), new IVect(1,-1)
            };



            public PerlinMap(int PerlinWidth = 32)
            {
                this.PerlinWidth = PerlinWidth;
                vectors = new Vector2[SectorSize / PerlinWidth + 1, SectorSize / PerlinWidth + 1];

                GenVectors();
            }



            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            public void GenVectors()
            {

                for (int x = 0; x < SectorSize / PerlinWidth + 1; x++)
                {
                    for (int y = 0; y < SectorSize / PerlinWidth + 1; y++)
                    {
                        //int random = randy.Next(0, 4);
                        //vectors[x, y] = VectorPosibilities[random];
                        vectors[x, y] = randomGradient(x, y, RandSeed);
                    }
                }
            }





            static Vector2 randomGradient(int ix, int iy, int seed)
            {
                unchecked
                {
                    // No precomputed gradients mean this works for any number of grid coordinates
                    const int w = 8 * sizeof(int);
                    const int s = w / 2; // rotation width
                    uint a = (uint)ix, b = (uint)iy;
                    a *= 3284157443; b ^= a << s | a >> w - s;
                    b *= 1911520717; a ^= b << s | b >> w - s;
                    a *= 2048419325;
                    a ^= (uint)seed;
                    float random = a * (MathF.PI / ~(~0u >> 1)); // in [0, 2*Pi]
                    Vector2 v;
                    v.X = MathF.Cos(random); v.Y = MathF.Sin(random);
                    return v;
                }
            }













            #region PerlinValueCreation
            public float GetValue(IVect pos)
            {
                return GetValue(pos.x, pos.y);
            }
            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
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

                return Lerp(North, South, ((y - Squarey * PerlinWidth) / (float)PerlinWidth)) * 1.4f;
            }
            #endregion PerlinValueCreation

            [MethodImpl(MethodImplOptions.AggressiveOptimization)]
            public float Lerp(float a0, float a1, float w) // perc  is distance from A -> B
            {
                //return (a1 - a0) * w + a0;
                return (a1 - a0) * (3.0f - w * 2.0f) * w * w + a0;
                //return (a1 - a0) * ((w * (w * 6.0f - 15.0f) + 10.0f) * w * w * w) + a0;
            }
        }
    }
}
