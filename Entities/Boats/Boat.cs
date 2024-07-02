using System;
using System.Collections.Generic;
using System.Linq;
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
            public int Thrust { get; }
            public int TurnSpeed { get; }
            public int FrictionDivider { get; }
            public bool ThrustActive { get; set; }

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
            IVect MovedPos = player.pos - boat.pos;

            //IVect RotatedPos = new IVect(
            //    Math.Cos(boat.angle) * MovedPos.x + Math.Sin(boat.angle) * MovedPos.y,
            //    Math.Sin(boat.angle) * MovedPos.x + Math.Cos(boat.angle) * MovedPos.y
            //    );

            return
                (-boat.Width * 32 <= MovedPos.x && MovedPos.x <= boat.Width * 0) &&
                (-boat.Length * 32 + 16 <= MovedPos.y && MovedPos.y <= boat.Length * 0);
        }
    }
}
