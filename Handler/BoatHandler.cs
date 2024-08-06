using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Short_Tools;
using static Base_Building_Game.General;
using static Short_Tools.General;
using IVect = Short_Tools.General.ShortIntVector2;








namespace Base_Building_Game
{
    public static partial class General
    {
        public static void DoBoatHandles(string inp, bool down)
        {
            switch (inp)
            {
                case "SPACE":

                    if (player.boat is Boat Cboat)
                    {
                        Cboat.Pilot = null;
                        player.boat = null;
                    }
                    if (player.turret is Turret CTurret)
                    {
                        CTurret.Pilot = null;
                        player.turret = null;
                    }

                    break;





                case "e":

                    if (!down || !MenuState.IsInGame()) { return; }

                    IVect MPos = getMousePos();

                    foreach (Boat boat in (from entity in LoadedActiveEntities where entity is Boat select (Boat)entity).ToArray())
                    {
                        Vector2 blockMousePos = new Vector2(renderer.GetFBlockx(MPos.x), renderer.GetFBlocky(MPos.y));

                        if (MathF.Abs(boat.pos.X - blockMousePos.X) < 0.5f &&
                            MathF.Abs(boat.pos.Y - blockMousePos.Y) < 0.5f)
                        {
                            boat.Pilot = player;
                            player.boat = boat;
                            player.turret = null;
                            break;
                        }
                    }



                    foreach (Turret turret in (from entity in LoadedActiveEntities where entity is Turret select (Turret)entity).ToArray())
                    {
                        Vector2 blockMousePos = new Vector2(renderer.GetFBlockx(MPos.x), renderer.GetFBlocky(MPos.y));

                        if (MathF.Abs(turret.pos.X - blockMousePos.X + 0.5f) < 0.5f &&
                            MathF.Abs(turret.pos.Y - blockMousePos.Y + 0.5f) < 0.5f)
                        {
                            turret.Pilot = player;
                            player.boat = null;
                            player.turret = turret;
                            break;
                        }
                    }

                    break;




                case "Mouse":

                    if (!down) { break; }

                    if (player.turret is Turret Mturret)
                    {
                        if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - Mturret.LastFireTime > Mturret.TimePerFire)
                        {
                            Mturret.LastFireTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                            Projectile bullet = new Projectile(Mturret.pos, Mturret.angle);
                            LoadedActiveEntities.Add(bullet);
                        }
                    }

                    break;
            } 
        }
    }
}
