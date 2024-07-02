using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base_Building_Game
{
    public static partial class General
    {
        public interface IActiveEntity : IEntity
        {
            public void Action();
        }

        static List<IActiveEntity> LoadedActiveEntities = new List<IActiveEntity>();
        public static void RunActiveEntities()
        {
            foreach (IActiveEntity entity in LoadedActiveEntities)
            {
                entity.Action();
            }
        }
    }
}
