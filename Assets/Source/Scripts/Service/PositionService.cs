using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.ViewComponent;
using Source.Scripts.System.Util;
using Source.Scripts.View;

namespace Source.Scripts.Service
{
    public class PositionService
    {
        private EcsWorld world;
        private readonly SaveData save;
        private readonly GameData gameData;
        private readonly GameConfig config;
        private readonly Pools pool;
        

        public PositionService(EcsWorld world, SaveData save, GameData gameData, GameConfig config,Pools pool)
        {
            this.world = world;
            this.save = save;
            this.gameData = gameData;
            this.config = config;
            this.pool = pool;
            
        }

        public List<int> GetEntInRadius(int startEnt,EcsFilter filter,float radius)
        {
            var list = new List<int>();
            var startTr = pool.View.Get(startEnt).Value.transform;
            foreach (var ent in filter)
            {
                var transform = pool.View.Get(ent).Value.transform;
                if ((transform.position-startTr.position).magnitude<= radius)
                {
                    list.Add(ent);
                }
            }

            return list;
        }
    }
}