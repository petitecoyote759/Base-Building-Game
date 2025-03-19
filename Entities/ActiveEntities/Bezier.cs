using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Drawing.Drawing2D;
using System.Diagnostics.CodeAnalysis;

namespace Base_Building_Game
{
    public static partial class General
    {

        internal static class Bezier
        {
            public static Queue<Vector2> GetBezier(Queue<Vector2> inputPath, float tStep = 0.2f)
            {
                if (inputPath.Count == 0) { return inputPath; }

                Queue<Vector2> path = new Queue<Vector2>();
                
                if (inputPath.Count % 3 != 0) { inputPath.Enqueue(inputPath.Last()); }
                //if (inputPath.Count % 3 != 0) { inputPath.Enqueue(inputPath.Peek()); }


                float t = 0f;
                float j = 1f;


                Vector2 firstPathPoint = inputPath.Dequeue();
                Vector2 secondPathPoint = inputPath.Dequeue();
                Vector2 pastPoint = firstPathPoint;
                Vector2 futurePoint = (firstPathPoint + secondPathPoint) / 2f;
                // when go past future point,
                // pastPoint = futurePoint;
                // firstPathPoint = secondPathPoint;
                // secondPathPoint = inputPath.Pop();
                // futurePoint = mean of first and second path point;


                while (inputPath.Count > 0)
                {
                    for (t = 0; t < 1f; t += tStep)
                    {
                        j = 1 - t;
                        path.Enqueue(
                            t * t * futurePoint +
                            2 * t * j * firstPathPoint +
                            j * j * pastPoint
                            );
                    }

                    pastPoint = futurePoint;
                    firstPathPoint = secondPathPoint;
                    secondPathPoint = inputPath.Dequeue();
                    futurePoint = (firstPathPoint + secondPathPoint) / 2f;
                }
                for (t = 0; t < 1f; t += tStep)
                {
                    j = 1 - t;
                    path.Enqueue(
                        t * t * futurePoint +
                        2 * t * j * firstPathPoint +
                        j * j * pastPoint
                        );
                }
                Vector2 midEndPoint = (secondPathPoint + futurePoint) / 2f;

                for (t = 0; t < 1f; t += tStep)
                {
                    j = 1 - t;
                    path.Enqueue(
                        t * t * secondPathPoint +
                        2 * t * j * midEndPoint +
                        j * j * futurePoint
                        );
                }
                path.Enqueue(secondPathPoint);

                return path;
            }
        }

    }
}
