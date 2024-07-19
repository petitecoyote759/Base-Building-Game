using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;




#pragma warning disable CS8618 // property turrets needs to be defined, i should probably do this
#warning Turrets should be defined M ;(





namespace Base_Building_Game
{
    public static partial class General
    {
        public class Skiff : Boat
        {
            public Vector2 pos { get; set; } 

            public double angle { get; set; } = 0d;
            public float Thrust { get; } = 0.0008f;
            public float TurnSpeed { get; } = 3;

            public float FrictionCoeffic { get; } = 0.005f;
            public bool ThrustActive { get; set; } = false;
            public IEntity? Pilot { get; set; }

            public short ID { get => (short)BoatID.Skiff; }


            const int HealthPerTier = 1000;
            const int BaseHealth = 2000;

            public int Width { get => 1; }
            public int Length { get => 2; }


            public int Weight { get => 100; }

            public Vector2 velocity { get; set; } = new IVect();

            public Turret[] turrets { get; set; } 




            public int MaxHealth { get => BaseHealth + (HealthPerTier * BoatResearch[ID]); }
            public int CurrentHealth { get; set; }

            public Dictionary<short, int> ResourceCosts { get; } = new Dictionary<short, int>()
            {

            };



            public Skiff(IVect pos)
            {
                CurrentHealth = MaxHealth;
                this.pos = pos;
            }

            public Skiff()
            {
                pos = new IVect(int.MinValue, int.MinValue);
            }






            public void Action(int dt)
            {
                MoveBoat(this, dt);
            }
        }



        public static int SignOf(float x)
        {
            if (x < 0) { return -1; }
            else if (x == 0) { return 0; }
            else { return 1; }
        }
    }
}
