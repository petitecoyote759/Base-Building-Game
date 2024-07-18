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
        public class Leg : ISHasPosition
        {
            public Vector2 pos { get; set; } = new Vector2();

            public float x { get => pos.X; }
            public float X { get => pos.X; }
            public float y { get => pos.Y; }
            public float Y { get => pos.Y; }

            public PositionAnimation<Leg>? animation;


            public Leg()
            {
                pos = new Vector2();
            }
            public Leg(Vector2 pos)
            {
                this.pos = pos;
            }



            public static implicit operator Vector2(Leg leg) => leg.pos;
            public static implicit operator Leg(Vector2 pos) => new Leg() { pos = pos };
        }
    }
}
