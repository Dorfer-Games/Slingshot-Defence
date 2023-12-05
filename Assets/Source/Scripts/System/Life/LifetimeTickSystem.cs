using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using UnityEngine;

namespace Source.Scripts.System.Life
{
    public class LifetimeTickSystem : GameSystem
    {
        private EcsFilter filter;
        
        public override void OnInit()
        {
            base.OnInit();
            filter = world.Filter<Lifetime>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var ent in filter)
            {
               ref var time =ref pool.Lifetime.Get(ent).Value;
               if (time <= 0)
               {
                   pool.Dead.Add(ent);
               }
               else
                   time -= Time.deltaTime;
            }
        }
    }
}