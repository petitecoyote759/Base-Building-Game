using System;
using System.Collections.Generic;
using System.Linq;
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
            public IVect pos { get; set; } 

            public double angle { get; set; } = 0d;
            public int Thrust { get; } = 100;
            public int TurnSpeed { get; } = 10;

            public int FrictionDivider { get; } = 5;
            public bool ThrustActive { get; set; } = false;
            public bool HasPlayerPilot { get; set; } = false;

            public short ID { get => (short)BoatID.Skiff; }


            const int HealthPerTier = 1000;
            const int BaseHealth = 2000;

            public int Width { get => 1; }
            public int Length { get => 2; }


            public int Weight { get => 100; }

            public void Action(int dt)
            {

            }

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
        }
    }
}
