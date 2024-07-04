using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;
using System.Numerics;




namespace Base_Building_Game
{
    public static partial class General
    {
        public class Projectile : IActiveEntity
        {
            public Vector2 pos { get; set; }


            float speed  = 0.01f;

            public double angle;
            public Vector2 dir;

            long spawnTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            const long LifeSpan = 3000;



            public Projectile(Vector2 pos, double angle)
            {
                this.pos = pos + new Vector2(0.5f, 0.5f);
                this.angle = angle;
                dir = new Vector2(
                    MathF.Cos((float)(angle - 90) * MathF.PI / 180f),
                    MathF.Sin((float)(angle - 90) * MathF.PI / 180f)
                    );
            }






            public void Action(int dt)
            {
                pos += dir * dt * speed;

                if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - spawnTime > LifeSpan)
                {
                    LoadedActiveEntities.Remove(this);
                }
            }
        }
    }
}
