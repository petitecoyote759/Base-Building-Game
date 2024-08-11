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
        public class DropInAni : Animation
        {
            public bool Finished { get; set; } = false;
            public object ObjGetter { get => renderer.zoom; }

            const int TargetZoom = 64;
            const int InitialZoom = 4;

            float speed = 0.2f;

            long dtSum = 0;

            Action<long> TempTick;


            public enum TickTypes : int
            {
                Linear,
                Root,
                Expo,
                Sigmoid
            }



            public DropInAni()
            {
                TempTick = RootTick;
                renderer.animations.Add(this);
            }



            public void Tick(long dt)
            {
                TempTick(dt);
                Print(renderer.zoom);
            }























            private void LinTick(long dt)
            {
                dtSum += dt;

                float ratio = speed * dtSum / 1000f;
                renderer.zoom = (int)(ratio * (TargetZoom - InitialZoom) + InitialZoom);

                if (ratio >= 1)
                {
                    renderer.zoom = TargetZoom;
                    Finished = true;
                }
            }


            private void ExpoTick(long dt)
            {
                dtSum += dt;

                float ratio = (float)Math.Pow(2, (speed * dtSum / 1000f) * (speed * dtSum / 1000f)) - 1;
                renderer.zoom = (int)(ratio * (TargetZoom - InitialZoom) + InitialZoom);

                if (ratio >= 1)
                {
                    renderer.zoom = TargetZoom;
                    Finished = true;
                }
            }

            private void RootTick(long dt)
            {
                dtSum += dt;

                float ratio = (float)Math.Sqrt(speed * dtSum / 1000f);
                renderer.zoom = (int)(ratio * (TargetZoom - InitialZoom) + InitialZoom);

                if (ratio >= 1)
                {
                    renderer.zoom = TargetZoom;
                    Finished = true;
                }
            }

            private void SigTick(long dt)
            {
                dtSum += dt;

                float x = speed * dtSum / 1000f;
                float ratio = x * x * x * (10 + x * (6 * x - 15));
                renderer.zoom = (int)(ratio * (TargetZoom - InitialZoom) + InitialZoom);

                if (ratio >= 1)
                {
                    renderer.zoom = TargetZoom;
                    Finished = true;
                }
            }
        }
    }
}
