using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.Event;

namespace Source.Scripts.System.Move
{
    public class KnockbackFromDamageSystem : GameSystem
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
                var sender = damageEvent.Sender;
                var target = damageEvent.Target;

                if (damageEvent.Damage <= 0)
                    continue;

                if (!pool.Rb.Has(target))
                    continue;
                
                if (!pool.Knockback.Has(sender))
                    continue;

                var force = pool.Knockback.Get(sender).Value;

                ref var knockbackEvent = ref pool.KnockbackEvent.Add(eventWorld.NewEntity());
                knockbackEvent.Sender = sender;
                knockbackEvent.Target = target;
                knockbackEvent.Force = force;
            }
            
        }
       
       
    }
}