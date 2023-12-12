using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Event;
using Source.Scripts.Component.Movement;
using Source.Scripts.Component.ViewComponent;
using UnityEngine;

namespace Source.Scripts.System.Move
{
    public class CheckDeadSystem : GameSystem
    {
        private EcsFilter filter;
        
        public override void OnInit()
        {
            base.OnInit();

            filter = world.Filter<Hp>().Exc<DeadTag>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var ent in filter)
            {
                ref var hp =ref pool.Hp.Get(ent);
                if ( hp.CurHp <=0)
                {
                    pool.Dead.Add(ent);
                    pool.CantMove.GetOrCreateRef(ent);
                }
            }
        }

    }
}