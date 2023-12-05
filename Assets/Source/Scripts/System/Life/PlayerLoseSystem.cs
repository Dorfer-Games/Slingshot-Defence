using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Event;

namespace Source.Scripts.System.Battle
{
    public class PlayerLoseSystem : GameSystem
    {
        private EcsFilter filter;

        public override void OnInit()
        {
            base.OnInit();
            filter = world.Filter<DeadTag>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
          
            foreach (var ent in filter)
            {
                if (ent==game.PlayerEntity)
                {
                    pool.Dead.Del(ent);
                    //show lose screen
                }
            }
           
        }
    }
}