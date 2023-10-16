using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Event;
using Source.Scripts.Component.Movement;
using Source.Scripts.Component.ViewComponent;
using UnityEngine;

namespace Source.Scripts.System.Move
{
    public class DestroyDeadSystem : GameSystem
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
                if (pool.View.Has(ent))
                {
                    pool.View.Get(ent).Value.Die();
                    world.DelEntity(ent);
                }
            }
        }

    }
}