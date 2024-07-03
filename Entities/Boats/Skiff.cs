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
            public float Thrust { get; } = 0.0005f;
            public float TurnSpeed { get; } = 3;

            public float FrictionCoeffic { get; } = 0.005f;
            public bool ThrustActive { get; set; } = false;
            public bool HasPlayerPilot { get; set; } = false;

            public short ID { get => (short)BoatID.Skiff; }


            const int HealthPerTier = 1000;
            const int BaseHealth = 2000;

            public int Width { get => 1; }
            public int Length { get => 2; }


            public int Weight { get => 100; }

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
                Vector2 RForce = new Vector2();

                
                RForce -= new Vector2(
                    SignOf(velocity.X) * MathF.Pow(velocity.X + (SignOf(velocity.X) / 10f), 2) * FrictionCoeffic,
                    SignOf(velocity.Y) * MathF.Pow(velocity.Y + (SignOf(velocity.Y) / 10f), 2) * FrictionCoeffic
                    );



                if (ThrustActive)
                {
                    RForce -= new Vector2(
                        (-Thrust * MathF.Sin(((float)angle) * MathF.PI / 180f)),
                        (Thrust * MathF.Cos(((float)angle) * MathF.PI / 180f))
                        ) * ((float)dt * 0.1f);
                }


                Vector2 oldVel = velocity;

                velocity += RForce * dt;





                if (oldVel.X < 0 && velocity.X > 0)
                {
                    velocity = new Vector2(0, velocity.Y);
                }
                if (oldVel.X > 0 && velocity.X < 0)
                {
                    velocity = new Vector2(0, velocity.Y);
                }


                if (oldVel.Y < 0 && velocity.Y > 0)
                {
                    velocity = new Vector2(velocity.X, 0);
                }
                if (oldVel.Y > 0 && velocity.Y < 0)
                {
                    velocity = new Vector2(velocity.X, 0);
                }





                pos += velocity * dt / Weight;




                world.Walkable(player.pos);
                if (player.boat == this)
                {
                    player.pos += velocity * dt / Weight;
                }





                if (HasPlayerPilot)
                {
                    player.pos = pos;


                    if (ActiveKeys["a"])
                    {
                        angle -= TurnSpeed * Math.Min(velocity.Length() * 2, 0.01f) * dt;
                    }
                    if (ActiveKeys["d"])
                    {
                        angle += TurnSpeed * Math.Min(velocity.Length() * 2, 0.01f) * dt;
                    }
                }
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
