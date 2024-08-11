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
        public class Battleship : Boat
        {
            public Vector2 pos { get; set; }

            public double angle { get; set; } = 0d;
            public float Thrust { get; } = 0.001f;
            public float TurnSpeed { get; } = 1;

            public float FrictionCoeffic { get; } = 0.001f;
            public bool ThrustActive { get; set; } = false;
            public IEntity? Pilot { get; set; }

            public short ID { get => (short)BoatID.Battleship; }


            const int HealthPerTier = 1000;
            const int BaseHealth = 2000;

            public int Width { get => 3; }
            public int Length { get => 6; }


            public int Weight { get => 400; }

            public Vector2 velocity { get; set; } = new IVect();

            public Turret[] turrets { get; set; }


            public int MaxHealth { get => BaseHealth + (HealthPerTier * BoatResearch[ID]); }
            public int CurrentHealth { get; set; }

            public Dictionary<short, int> ResourceCosts { get; } = new Dictionary<short, int>()
            {

            };



            public Battleship(IVect pos)
            {
                CurrentHealth = MaxHealth;
                this.pos = pos;
                turrets = Array.Empty<Turret>(); // TODO: Add turrets here
            }

            public Battleship()
            {
                pos = new IVect(int.MinValue, int.MinValue);
                turrets = Array.Empty<Turret>();
            }






            public void Action(int dt)
            {
                MoveBoat(this, dt);
            }
        }
    }
}
