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
                    HandleRicochet(sender, targets);
                }else if (pool.Through.Has(sender))
                {
                    HandleThrough(sender, targets);
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

        private void HandleThrough(int sender, List<int> targets)
        {
            ref var through = ref pool.Through.Get(sender);
            if (through.Value <= 0)
                pool.Dead.Add(sender);
            else
                through.Value--;
        }
        private void HandleRicochet(int sender,List<int> targets)
        {
            ref var ricochet = ref pool.Ricochet.Get(sender);

            if (ricochet.Count<=0)
            {
                pool.Dead.Add(sender);
            }
            else
            {
                ricochet.Count--;
                var newTargets =
                    game.PositionService.GetEnemiesInRadiusWithPriority(sender, ricochet.Radius, targets,
                        false);
                if (newTargets.Count>0)
                {
                    var newTarget = newTargets[0];
                }
            }
        }
    }
}