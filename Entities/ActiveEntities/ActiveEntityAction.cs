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
            public void Action(int dt);
        }

        static List<IActiveEntity> LoadedActiveEntities = new List<IActiveEntity>();
        public static void RunActiveEntities(int dt)
        {
            foreach (IActiveEntity entity in LoadedActiveEntities)
            {
                entity.Action(dt);
            }
        }
    }
}
