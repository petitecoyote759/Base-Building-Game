using Short_Tools;
using System.Numerics;
using static Short_Tools.General;
using static System.MathF;
using static System.Numerics.Vector2;
using IVect = Short_Tools.General.ShortIntVector2;
using Priority = Short_Tools.ShortDebugger.Priority;
using V2 = System.Numerics.Vector2;




namespace Base_Building_Game
{
    public static partial class General
    {
        public interface IWeapon
        {
            public int Damage { get; }

            /// <summary>
            /// Speed of the bullets in blocks, -1 for hitscan
            /// </summary>
            public float BulletSpeed { get; }
            public float Range { get; }
            public bool Melee { get; }
        }





        public struct Sword : IWeapon
        {
            public int Damage { get => 10; }
            public float BulletSpeed { get => -1; }
            public float Range { get => 1; }
            public bool Melee { get => true; }
        }
    }
}