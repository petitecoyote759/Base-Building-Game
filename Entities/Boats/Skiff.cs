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
        public class Skiff : Boat
        {
            public Vector2 pos { get; set; } 

            public double angle { get; set; } = 0d;
            public float Thrust { get; } = 0.2f;
            public int TurnSpeed { get; } = 10;

            public float FrictionCoeffic { get; } = 0.4f;
            public bool ThrustActive { get; set; } = false;
            public bool HasPlayerPilot { get; set; } = false;

            public short ID { get => (short)BoatID.Skiff; }


            const int HealthPerTier = 1000;
            const int BaseHealth = 2000;

            public int Width { get => 1; }
            public int Length { get => 2; }


            public int Weight { get => 20; }

            public Vector2 velocity { get; set; } = new IVect();




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

                if (ThrustActive)
                {
                    velocity -= new Vector2(
                        (-Thrust * MathF.Sin(((float)angle) * MathF.PI / 180f)),
                        (Thrust * MathF.Cos(((float)angle) * MathF.PI / 180f))
                        ) * ((float)dt * 0.001f / Weight);
                }
                else
                {
                    velocity = new Vector2();
                }

                pos = pos + velocity * dt;


                if (HasPlayerPilot)
                {
                    player.pos = pos;


                    if (ActiveKeys["a"])
                    {
                        angle -= 4;
                    }
                    if (ActiveKeys["d"])
                    {
                        angle += 4;
                    }
                }
            }
        }
    }
}
