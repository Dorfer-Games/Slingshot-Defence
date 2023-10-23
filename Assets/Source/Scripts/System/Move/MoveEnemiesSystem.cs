using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Movement;
using Source.Scripts.Component.ViewComponent;
using UnityEngine;
using UnityEngine.AI;

namespace Source.Scripts.System.Move
{
    public class MoveEnemiesSystem : GameSystem
    {
        private EcsFilter filter;
       

        public override void OnInit()
        {
            base.OnInit();

            filter = world.Filter<Direction>().Inc<Moveable>().Inc<Speed>().Inc<BaseViewComponent>().Exc<RigidbodyComponent>().Exc<CantMoveTag>().Exc<NavMeshAgentComponent>().End();
          
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var ent in filter)
            {
                var valueTransform = pool.View.Get(ent).Value.transform;
                var dir = pool.Dir.Get(ent).Value;
                var speed = pool.Speed.Get(ent).Value;
                valueTransform.Translate(dir*(Time.deltaTime*speed));
            }
            
           
        }
        
    }
}