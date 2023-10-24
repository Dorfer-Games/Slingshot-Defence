using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.Event;

namespace Source.Scripts.System.Battle
{
    public class HitSystem : GameSystem
    {
        private EcsFilter filter;

        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<HitEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            var list = new List<int>();
            foreach (var e in filter)
            {
                var hitEvent = pool.HitEvent.Get(e);
                var sender = hitEvent.Sender;
                var targets = hitEvent.Targets;

                if (list.Contains(sender))
                    continue;

                if (pool.Ricochet.Has(sender))
                {
                    
                }else if (pool.Through.Has(sender))
                {
                    
                }
                else
                {
                    pool.Dead.Add(sender);
                }

                //var elementType = pool.Element.Get(sender).Value;
                foreach (var target in targets)
                {
                    ref var damageEvent = ref pool.DamageEvent.Add(eventWorld.NewEntity());
                    damageEvent.Damage = pool.Damage.Get(sender).Value;
                    damageEvent.Sender = sender;
                    damageEvent.Target = target;
                    //proc element
                }
              

                list.Add(sender);
            }
            list.Clear();
        }
    }
}