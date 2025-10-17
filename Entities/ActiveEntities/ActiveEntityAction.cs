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

        static readonly List<IActiveEntity> LoadedActiveEntities = new List<IActiveEntity>();
        public static void RunActiveEntities(int dt)
        {
            IActiveEntity[] tempActiveEntities;

            lock (LoadedActiveEntities)
            {
                tempActiveEntities = LoadedActiveEntities.ToArray();
            }
            foreach (IActiveEntity entity in tempActiveEntities)
            {
                if (entity is null) { continue; } // TODO: fix this, why on earth is it null???
                if (Math.Abs(entity.pos.X - player.pos.X) + Math.Abs(entity.pos.Y - player.pos.Y) > 200) { continue; }
                entity.Action(dt);
            }
        }









        static Thread entityController;
        public static void EntityThreadLoader()
        {
            entityController = new Thread(new ThreadStart(EntityThreadController));  
            entityController.Start(); 
        }



        public static void EntityThreadController()
        {
            long dt;
            long LFT = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            while (Running)
            {
                if (renderer.ActiveCutscene is not null) { Thread.Sleep(50); continue; }

                dt = Short_Tools.General.GetDt(ref LFT);
                RunActiveEntities((int)dt);
            }
        }
    }
}
