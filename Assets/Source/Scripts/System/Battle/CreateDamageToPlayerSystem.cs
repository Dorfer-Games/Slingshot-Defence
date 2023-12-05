using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.Event;

namespace Source.Scripts.System.Battle
{
    public class CreateDamageToPlayerSystem : GameSystem
    {
        private EcsFilter filter;

        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<HitPlayerEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
         
            foreach (var e in filter)
            {
                var hitEvent = pool.HitPlayerEvent.Get(e);
                var sender = hitEvent.Sender;
                var target = game.PlayerEntity;
                
                ref var damageEvent = ref pool.DamageEvent.Add(eventWorld.NewEntity());
                damageEvent.Sender = sender;
                damageEvent.Target = target;
                damageEvent.Damage = pool.Hp.Get(sender).CurHp * config.HpToDmgK;

                pool.Dead.Add(sender);
            }
           
        }
    }
}