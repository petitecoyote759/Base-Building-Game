using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;
using static System.MathF;



namespace Base_Building_Game
{
    public static partial class General
    {
        public class Turret : IActiveEntity
        {
            public Boat boat;


            public Vector2 pos
            {
                get
                {
                    if (BoatID == (short)(General.BoatID.Destroyer))
                    {
                        return boat.pos + 1.35f * new Vector2(
                            Cos((float)(boat.angle + position * 180f - 90) * PI / 180f) - 0.375f, 
                            Sin((float)(boat.angle + position * 180f - 90) * PI / 180f) - 0.375f);
                    }

                    return new Vector2();
                }

                set { }
            }


            public IEntity? Pilot = null;

            public double angle 
            { 
                get 
                { 
                    if (Pilot == player) { InAngle = player.angle; return player.angle; }
                    return InAngle;
                } 
            }


            double InAngle = 0d;

            int position;
            short BoatID;




            public Turret(int position, Boat boat)
            {
                this.position = position;
                BoatID = boat.ID;
                this.boat = boat;
            }




            public void Action(int dt)
            {
                
            }
        }
    }
}
