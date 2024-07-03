using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;





namespace Base_Building_Game
{
    public static partial class General
    {
        public interface Boat : IActiveEntity
        {
            public double angle { get; set; }
            public float Thrust { get; }
            public float TurnSpeed { get; }
            public float FrictionCoeffic { get; }
            public bool ThrustActive { get; set; }

            public Vector2 velocity { get; set; }

            public int MaxHealth { get; }
            public int CurrentHealth { get; set; }
            public short ID { get; }

            public int Weight { get; }


            public int Width { get; }
            public int Length { get; }


            public Dictionary<short, int> ResourceCosts { get; }


            public bool HasPlayerPilot { get; set; }
        }



        public enum BoatID : short
        {
            Skiff = 1,
            Destroyer,
            Battleship
        }














        public static bool IsPlayerWithinHitbox(Boat boat, Player player)
        {
            Vector2 MovedPos = player.pos - boat.pos;

            Vector2 RotatedPos = new Vector2(
                MathF.Cos((float)boat.angle * MathF.PI / 180f) * MovedPos.X + MathF.Sin((float)boat.angle * MathF.PI / 180f) * MovedPos.Y,
                -MathF.Sin((float)boat.angle * MathF.PI / 180f) * MovedPos.X + MathF.Cos((float)boat.angle * MathF.PI / 180f) * MovedPos.Y
                );

            return
                (-boat.Width / 2f <= RotatedPos.X && RotatedPos.X <= boat.Width / 2f) &&
                (-boat.Length / 2f <= RotatedPos.Y && RotatedPos.Y <= boat.Length / 2f);
        }
    }
}
