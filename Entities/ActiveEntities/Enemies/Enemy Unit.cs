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
            float speed = 0.005f;
            public Vector2 pos { get => thisPos; set => thisPos = value; }
            public double angle = 0d;
            #endregion Pos

            #region Health
            public int MaxHealth { get => 200; } // research * 100 + 200?

            public int CurrentHealth { get; set; }
            #endregion Health

            #region Path

            Stack<Vector2>? path = null;

            IEntity? Target = null;
            float t = 0;

            bool Wandering = true;

            Vector2 MoveTarget;

            Vector2[]? nextPositions;




            public void GetPath(IVect target, int maxDist = 30) // block dist ykyk
            {
                path = new AStar(world.Walkable, target, pos).GetPath(maxDist);
            }

            #endregion Path

            #region Weapons

            IWeapon weapon = new Sword();

            public bool InWeaponRange()
            {
                if (Target is null) { return false; }

                Vector2 delta = pos - Target.pos;

                return delta.LengthSquared() < weapon.Range * weapon.Range;
            }

            #endregion Weapons



            public EnemyUnit() 
            {
                CurrentHealth = MaxHealth;
                LoadedActiveEntities.Add(this);
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
                    Wandering = false;
                    if (path is Stack<Vector2> Path && Path.Count == 0)
                    {
                        if (Wandering)
                        {
                            GetWanderPoint();
                        }
                        else
                        {
                            if (InWeaponRange())
                            {
                                Attack();
                            }
                            else
                            {
                                GetPath(target.pos);
                            }
                        }
                    }
                    else if (path is not null)
                    {
                        if (nextPositions is null)
                        {
                            AddNextNodes();
                        }
                        Move(dt);
                    }
                    else
                    {
                        GetPath(target.pos);
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
                AStar pather = new AStar(world.Walkable, player.pos, pos);

                if (pather.GetPath(50) is not null)
                {
                    Target = player;
                }
            }





            public void AddNextNodes()
            {
                if (path is null) { return; }

                int size = path.Count >= 3 ? 3 : path.Count + 1;
                nextPositions = new Vector2[size];
                nextPositions[0] = pos;
                for (int i = 1; i < size; i++)
                {
                    nextPositions[i] = path.Pop();
                }
            }




            public void Move(int dt)
            {
                float MovementScalar = 1f / speed;

                t = t + dt;
                if (t >= MovementScalar)
                {
                    t = (int)MovementScalar;
                }
                this.pos = Bézier.ComputeBézier(t / MovementScalar, nextPositions);
                if (t == MovementScalar)
                {
                    nextPositions = null;
                    t = 0;
                }
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