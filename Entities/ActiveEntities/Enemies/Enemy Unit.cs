using Short_Tools;
using System.Numerics;
using static Base_Building_Game.Entities.General;
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

            holy fuck
            my down key works


            probably have hmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm 




            if (path is not null) <- if there is a target


            */

            #endregion Action Plan


            public void Action(int dt)
            {

            }
        }
    }
}