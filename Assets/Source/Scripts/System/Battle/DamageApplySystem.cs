using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.Event;

namespace Source.Scripts.System.Battle
{
    public class DamageApplySystem : GameSystem
    {
        private EcsFilter filter;

        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<DamageEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
          
            foreach (var e in filter)
            {
               
                var damageEvent = pool.DamageEvent.Get(e);
                var target = damageEvent.Target;
                ref var targetHp = ref pool.Hp.Get(target);
                
                targetHp.CurHp -= damageEvent.Damage;
            }
           
        }
    }
}