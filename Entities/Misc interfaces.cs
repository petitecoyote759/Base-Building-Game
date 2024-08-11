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
        public interface IHasHealth
        {
            public int MaxHealth { get; }
            public int CurrentHealth { get; set; }
        }
    }
}