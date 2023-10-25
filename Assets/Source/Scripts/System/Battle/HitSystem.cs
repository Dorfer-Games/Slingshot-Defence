using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.Event;
using UnityEngine;

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

                if (!DetectHit(sender, targets))
                    continue;


                if (pool.Ricochet.Has(sender) && pool.Through.Has(sender))
                {
                    HandleThroughRicochet(sender);
                }
                else if (pool.Ricochet.Has(sender))
                {
                    HandleRicochet(sender);
                }
                else if (pool.Through.Has(sender))
                {
                    HandleThrough(sender);
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

        private bool DetectHit(int sender, List<int> targets)
        {
            bool detect = false;

            var hashSet = UnpackTargets(sender);
            //has new target
            foreach (var target in targets)
            {
                if (!hashSet.Contains(target))
                {
                    detect = true;
                    hashSet.Clear();
                    hashSet.UnionWith(targets);
                    break;
                }
            }

            //update hit targets
            HashSet<EcsPackedEntity> newSet = new();
            foreach (var ent in hashSet)
            {
                newSet.Add(world.PackEntity(ent));
            }

            pool.PrevHitTargets.Get(sender).Value = newSet;

            return detect;
        }

        private void HandleThrough(int sender)
        {
            ref var through = ref pool.Through.Get(sender);
            if (through.Value <= 0)
                pool.Dead.Add(sender);
            else
                through.Value--;
        }

        private void HandleThroughRicochet(int sender)
        {
            ref var through = ref pool.Through.Get(sender);
            if (through.Value <= 0)
            {
                pool.Through.Del(sender);
                HandleRicochet(sender);
            }
            else
                through.Value--;
        }

        private void HandleRicochet(int sender)
        {
            ref var ricochet = ref pool.Ricochet.Get(sender);

            if (ricochet.Count <= 0)
            {
                pool.Dead.Add(sender);
            }
            else
            {
                ricochet.Count--;
                var hitTargets = UnpackTargets(sender);
                var newTargets =
                    game.PositionService.GetEnemiesInRadiusWithPriority(sender, ricochet.Radius, hitTargets,
                        true);
                if (newTargets.Count > 0)
                {
                    var rnd = Random.Range(0, newTargets.Count);
                    var newTarget = newTargets[rnd];
                    var targetPos = pool.View.Get(newTarget).Value.transform.position;
                    var senderPos = pool.View.Get(sender).Value.transform.position;
                    var dir = (targetPos - senderPos).normalized;
                    dir.y = 0;
                    pool.Dir.Get(sender).Value = dir;
                }
                else
                {
                    pool.Dead.Add(sender);
                }
            }
        }

        private HashSet<int> UnpackTargets(int sender)
        {
            var hitTargets = pool.PrevHitTargets.Get(sender).Value;
            var hashSet = new HashSet<int>();
            foreach (var packedEntity in hitTargets)
            {
                if (packedEntity.Unpack(world, out int entity))
                    hashSet.Add(entity);
            }

            return hashSet;
        }
    }
}