using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
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


            public IEntity? Pilot { get; set; }

            public Turret[] turrets { get; set; }
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









        public static bool BoatCanMove(Boat boat)
        {
            return true;


            Vector2[] RectPoints = new Vector2[]
            {
                (boat.pos + 0.5f * new Vector2(
                    (float)(boat.Length * Math.Sin(boat.angle) + boat.Width * Math.Cos(boat.angle)),
                    (float)(boat.Length * Math.Cos(boat.angle) + boat.Width * Math.Sin(boat.angle))
                    )),


                (boat.pos + 0.5f * new Vector2(
                    (float)(boat.Length * Math.Sin(boat.angle) - boat.Width * Math.Cos(boat.angle)),
                    (float)(boat.Length * Math.Cos(boat.angle) + boat.Width * Math.Sin(boat.angle))
                    )),


                (boat.pos + 0.5f * new Vector2(
                    (float)(-boat.Length * Math.Sin(boat.angle) + boat.Width * Math.Cos(boat.angle)),
                    (float)(-boat.Length * Math.Cos(boat.angle) + -boat.Width * Math.Sin(boat.angle))
                    )),

                new Vector2()
            };

            RectPoints[3] = RectPoints[1] + RectPoints[2] - RectPoints[0];

            for (int index = 0; index < 4; index++)
            {
                int i = index; int j = (index + 1) % 4;
                Vector2 normalised = Vector2.Normalize(RectPoints[j]);

                for (float t = 0; t < 1; t += 0.05f)
                {
                    IVect point = RectPoints[i] + (t * normalised);
                    short ID = world.GetTile(point.x, point.y).ID;

                    if (ID == (short)TileID.Grass || ID == (short)TileID.Sand || ID == (short)TileID.DeepOcean)
                    {
                        return false;
                    }
                }
            }


            return true;
        }





















        public static void MoveBoat(Boat boat, int dt)
        {
            Vector2 RForce = new Vector2();


            RForce -= new Vector2(
                SignOf(boat.velocity.X) * MathF.Pow(boat.velocity.X + (SignOf(boat.velocity.X) / 10f), 2) * boat.FrictionCoeffic,
                SignOf(boat.velocity.Y) * MathF.Pow(boat.velocity.Y + (SignOf(boat.velocity.Y) / 10f), 2) * boat.FrictionCoeffic
                );



            if (boat.ThrustActive)
            {
                RForce -= new Vector2(
                    (-boat.Thrust * MathF.Sin(((float)boat.angle) * MathF.PI / 180f)),
                    (boat.Thrust * MathF.Cos(((float)boat.angle) * MathF.PI / 180f))
                    ) * ((float)dt * 0.1f);
            }


            Vector2 oldVel = boat.velocity;

            boat.velocity += RForce * dt;





            if (oldVel.X < 0 && boat.velocity.X > 0)
            {
                boat.velocity = new Vector2(0, boat.velocity.Y);
            }
            if (oldVel.X > 0 && boat.velocity.X < 0)
            {
                boat.velocity = new Vector2(0, boat.velocity.Y);
            }


            if (oldVel.Y < 0 && boat.velocity.Y > 0)
            {
                boat.velocity = new Vector2(boat.velocity.X, 0);
            }
            if (oldVel.Y > 0 && boat.velocity.Y < 0)
            {
                boat.velocity = new Vector2(boat.velocity.X, 0);
            }





            boat.pos += boat.velocity * dt / boat.Weight;




            world.Walkable(player.pos);
            if (player.boat == boat)
            {
                player.pos += boat.velocity * dt / boat.Weight;
            }


            if (boat.Pilot is IEntity pilot)
            {
                pilot.pos = boat.pos;
                player.angle = boat.angle;
            }
            else if (IsPlayerWithinHitbox(boat, player))
            {
                player.pos += boat.velocity * dt / boat.Weight;
            }




            if (boat.Pilot == player)
            {

                if (ActiveKeys["a"])
                {
                    boat.angle -= boat.TurnSpeed * Math.Min(boat.velocity.Length() * 2, 0.01f) * dt;
                }
                if (ActiveKeys["d"])
                {
                    boat.angle += boat.TurnSpeed * Math.Min(boat.velocity.Length() * 2, 0.01f) * dt;
                }
            }



            foreach (Turret turret in boat.turrets)
            {
                if (turret.Pilot is IEntity entity)
                {
                    entity.pos = turret.pos + new Vector2(0.5f, 0.5f);
                }
            }
        }




























        public static bool TestBoatCanMove(Boat boat)
        {
            return false;
            Vector2[] RectPoints = new Vector2[]
            {
                (boat.pos + 0.5f * new Vector2(
                    (float)(boat.Length * Math.Sin(boat.angle) + boat.Width * Math.Cos(boat.angle)),
                    (float)(boat.Length * Math.Cos(boat.angle) + boat.Width * Math.Sin(boat.angle))
                    )),


                (boat.pos + 0.5f * new Vector2(
                    (float)(boat.Length * Math.Sin(boat.angle) - boat.Width * Math.Cos(boat.angle)),
                    (float)(boat.Length * Math.Cos(boat.angle) + boat.Width * Math.Sin(boat.angle))
                    )),


                (boat.pos + 0.5f * new Vector2(
                    (float)(-boat.Length * Math.Sin(boat.angle) + boat.Width * Math.Cos(boat.angle)),
                    (float)(-boat.Length * Math.Cos(boat.angle) + -boat.Width * Math.Sin(boat.angle))
                    )),

                new Vector2()
            };

            RectPoints[3] = RectPoints[1] + RectPoints[2] - RectPoints[0];




            int Dist = (int)MathF.Sqrt(boat.Width * boat.Width + boat.Length * boat.Length) + 1;

            for (int x = (int)boat.pos.X - Dist; x < (int)boat.pos.X + Dist; x++)
            {
                for (int y = (int)boat.pos.Y - Dist; y < (int)boat.pos.Y + Dist; y++)
                {



                    foreach (Vector2 point in RectPoints)
                    {
                        Tile tile = world.GetTile(x, y);
                        if (tile.ID != (short)TileID.Grass && tile.ID != (short)TileID.Sand) { continue; }

                        if (x <= point.X && point.X <= x + 1 &&
                            y <= point.Y && point.Y <= y + 1)
                        {
                            return false;
                        }
                    }



                    Vector2[] MTilePoints = new Vector2[]
                    {

                    };


                    foreach (Vector2 point in MTilePoints)
                    {
                        //if (RectPoints)
                    }
                }
            }
        }
    }
}
