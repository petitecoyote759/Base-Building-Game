using Short_Tools;
using System.Drawing;
using System.Numerics;
using static Short_Tools.General;
using static System.MathF;
using static System.Numerics.Vector2;
using IVect = Short_Tools.General.ShortIntVector2;
using Priority = Short_Tools.ShortDebugger.Priority;
//using V2 = System.Numerics.Vector2;




namespace Base_Building_Game
{
    public static partial class General
    {
        public class EnemyUnit : IActiveEntity, IHasHealth
        {
            #region Pos
            Vector2 thisPos;
            public Vector2 pos { get => thisPos; set => thisPos = value; }
            #endregion Pos

            #region Health
            public int MaxHealth { get => 200; } // research * 100 + 200?

            public int CurrentHealth { get; set; }
            #endregion Health

            #region Path

            Stack<Vector2>? path = null;

            IEntity? Target = null;

            bool Wandering = true;




            public void GetPath(IVect target, int maxDist = 30) // block dist ykyk
            {
                path = new AStar(world.Walkable, target, pos).GetPath(maxDist);
            }

            #endregion Path



            public EnemyUnit() 
            {
                CurrentHealth = MaxHealth;
            }



            #region Action Plan

            /*
             
            Priority fingy

            1 -> kill player
            2 -> kill workcamp men if close
            3 -> kill buildings (need to deal with hub, have higher want but not too much)
            -if buildings in way -> kill those (eg. walls)


            cause we have 

            player
            boats
            buildings
            hub
            workcamp men??? -> probably could, could be cool if you have your supply lines cut
            some types kill planes? -> only boats
            

            so action should look like this ish

            holy
            my down key works


            have Entity? target -> since buildings are entities now we can do fun stuff

            hmmmmm Weapon????? class that has like range { get; } so you can do gun bullet and alla that



            if (wandering)
            {
                get target init

                if (no target still)
                {
                    get wander point init
                }
            }

            if (targeting)
            {
                if (at point)
                {
                    if (wandering)
                    {
                        get wander point init
                    }
                    else
                    {
                        shoot bullets init
                    }
                }
                else
                {
                    go to point init
                }
            }
            else
            {
                get target init

                if (no target still)
                {
                    wander around init
                }
            }



            every now and again, check if the path is valid?
                                 check if target is alive.


            what if cant get back to house, and has no targets -> just wander to fixed points near.

            */

            #endregion Action Plan


            public void Action(int dt)
            {
                if (Target is null != path is null)  // wuh woh
                {
                    debugger.AddLog("somin went wrong"); // <- add better log init
                    Target = null; path = null;
                }



                if (Wandering)
                {
                    GetTarget();

                    if (Target is null)
                    {
                        GetWanderPoint();
                    }
                }






                if (Target is IEntity target)
                {
                    if (true/*AtEnd()*/)
                    {
                        if (Wandering)
                        {
                            GetWanderPoint();
                        }
                        else
                        {
                            if (true/*WeaponInRange()*/)
                            {
                                Attack();
                            }
                            else
                            {
                                GetPath(target.pos);
                            }
                        }
                    }
                    else
                    {
                        //Bézier.Move();
                    }
                }

                else 
                {
                    GetTarget();

                    if (Target is /*still!!!*/ null)
                    {
                        Wandering = true;
                    }
                }
            }








            public void GetTarget()
            {

            }





            public void GetWanderPoint()
            {

            }



            public void Attack()
            {

            }
        }
    }
}