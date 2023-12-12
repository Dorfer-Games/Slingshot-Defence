using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle.Tome;
using Source.Scripts.Component.Event;
using UnityEngine;

namespace Source.Scripts.System.Move
{
    public class KnockbackSystem : GameSystem
    {
        private EcsFilter filter;
        private EcsFilter filterTick;
        
        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<KnockbackEvent>().End();
            filterTick = world.Filter<KnockedTick>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filter)
            {
                var damageEvent = pool.KnockbackEvent.Get(e);
                var target = damageEvent.Target;
                var force = damageEvent.Force;
                
                pool.KnockedTick.GetOrCreateRef(target).Value = config.KnockbackTime;
                pool.NavMeshAgentComponent.Get(target).Value.enabled = false;
             
                var tr = pool.View.Get(target).Value.transform;

                var rb = pool.Rb.Get(target).Value;
                rb.constraints =RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
                
                rb.AddExplosionForce(force, tr.position+tr.forward+tr.up, 0,0,ForceMode.VelocityChange);
            }

            foreach (var ent in filterTick)
            {
                ref var knockedTick = ref pool.KnockedTick.Get(ent);
                if (knockedTick.Value<=0)
                {
                    pool.KnockedTick.Del(ent);
                    var rb = pool.Rb.Get(ent).Value;
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    if (pool.DeathAnimTick.Has(ent))
                        continue;
                        
                    pool.NavMeshAgentComponent.Get(ent).Value.enabled = true;
                    pool.NavMeshAgentComponent.Get(ent).Value.SetDestination(game.PlayerView.transform.position);
                   
                }
                else
                {
                    knockedTick.Value -= Time.deltaTime;
                }

            }
        }
       
       
    }
}