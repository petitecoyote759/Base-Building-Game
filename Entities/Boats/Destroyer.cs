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
        public class Destroyer : Boat
        {
            public Vector2 pos { get; set; }

            public double angle { get; set; } = 0d;
            public float Thrust { get; } = 0.0005f;
            public float TurnSpeed { get; } = 3; // 3

            public float FrictionCoeffic { get; } = 0.005f; // 0.005f
            public bool ThrustActive { get; set; } = false;
            public IEntity? Pilot { get; set; }

            public short ID { get => (short)BoatID.Destroyer; }


            const int HealthPerTier = 1000;
            const int BaseHealth = 2000;

            public int Width { get => 2; }
            public int Length { get => 4; }


            public int Weight { get => 100; }

            public Vector2 velocity { get; set; } = new IVect();

            public Turret[] turrets { get; set; }




            public int MaxHealth { get => BaseHealth + (HealthPerTier * BoatResearch[ID]); }
            public int CurrentHealth { get; set; }

            public Dictionary<short, int> ResourceCosts { get; } = new Dictionary<short, int>()
            {

            };



            public Destroyer(IVect pos)
            {
                CurrentHealth = MaxHealth;
                this.pos = pos;

                turrets = new Turret[2];

                turrets[0] = new Turret(0, this);
                LoadedActiveEntities.Add(turrets[0]);
                turrets[1] = new Turret(1, this);
                LoadedActiveEntities.Add(turrets[1]);
            }

            public Destroyer()
            {
                pos = new IVect(int.MinValue, int.MinValue);
            }






            public void Action(int dt)
            {
                MoveBoat(this, dt);
            }
        }
    }
}
