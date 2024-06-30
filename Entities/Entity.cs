using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IVect = Short_Tools.General.ShortIntVector2;

namespace Base_Building_Game
{
    public static partial class General
    {
        static List<IEntity> LoadedEntities = new List<IEntity>();
        /// <summary>
        /// The general interface for all entities.
        /// </summary>
        public interface IEntity
        {
            IVect pos { get; set; } 
        }
    }

}
