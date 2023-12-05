using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle.Tome;
using Source.Scripts.Component.Movement;
using Source.Scripts.Component.ViewComponent;
using UnityEngine;
using UnityEngine.AI;

namespace Source.Scripts.System.Move
{
    public class MoveApplySystem : GameSystem
    {
        private EcsFilter filter;
        private EcsFilter filterRot;
        private EcsFilter filterRb;
        private EcsFilter filterRbStop;
        private EcsFilter filterRbStop1;
        private EcsFilter filterRbRot;
        
        private EcsFilter filterAgent;

        public override void OnInit()
        {
            base.OnInit();

            filter = world.Filter<Direction>().Inc<MaxSpeed>().Inc<Speed>().Inc<BaseViewComponent>().Exc<RigidbodyComponent>().Exc<CantMoveTag>().Exc<NavMeshAgentComponent>().Exc<KnockedTick>().End();
            filterRot = world.Filter<Direction>().Inc<MaxSpeed>().Inc<BaseViewComponent>().Exc<RigidbodyComponent>().Exc<CantMoveTag>().Exc<NavMeshAgentComponent>().End();
            filterRb = world.Filter<Direction>().Inc<MaxSpeed>().Inc<Speed>().Inc<BaseViewComponent>().Inc<RigidbodyComponent>().Exc<NavMeshAgentComponent>().Exc<KnockedTick>().End();
            filterRbRot = world.Filter<Direction>().Inc<MaxSpeed>().Inc<BaseViewComponent>().Inc<RigidbodyComponent>().Exc<NavMeshAgentComponent>().End();
            filterRbStop = world.Filter<Direction>().Inc<MaxSpeed>().Inc<BaseViewComponent>().Inc<RigidbodyComponent>().Exc<Speed>().Exc<NavMeshAgentComponent>().End();
            filterRbStop1 = world.Filter<Direction>().Inc<MaxSpeed>().Inc<CantMoveTag>().Inc<BaseViewComponent>().Inc<RigidbodyComponent>().Exc<NavMeshAgentComponent>().End();

            filterAgent = world.Filter<Direction>().Inc<MaxSpeed>().Inc<BaseViewComponent>().Inc<Speed>()
                .Inc<NavMeshAgentComponent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var ent in filter)
            {
                var valueTransform = pool.View.Get(ent).Value.transform;
                var dir = pool.Dir.Get(ent).Value;
                var speed = pool.Speed.Get(ent).Value;
                valueTransform.position+=(dir*(Time.deltaTime*speed));
            }
            
            foreach (var ent in filterRot)
            {
                var valueTransform = pool.View.Get(ent).Value.transform;
                var dir = pool.Dir.Get(ent).Value;
                valueTransform.rotation=Quaternion.LookRotation(dir);
            }

            foreach (var ent in filterAgent)
            {
                var speed = pool.Speed.Get(ent).Value;
                pool.NavMeshAgentComponent.Get(ent).Value.speed = speed;
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            foreach (var ent in filterRb)
            {
                var dir = pool.Dir.Get(ent).Value;
                var speed = pool.Speed.Get(ent).Value;
                var rb = pool.Rb.Get(ent).Value;
                //rb.velocity = Vector3.Lerp(rb.velocity,dir * speed,Time.fixedTime);
                rb.velocity =dir * speed;
            }

            foreach (var ent in filterRbRot)
            {
                var dir = pool.Dir.Get(ent).Value;
                var rb = pool.Rb.Get(ent).Value;
             
                var targetRot=Quaternion.LookRotation(dir);
                rb.MoveRotation( Quaternion.Slerp(rb.rotation,targetRot,config.UnitRotSpeed));
            }
            
            foreach (var ent in filterRbStop)
            {
                pool.Rb.Get(ent).Value.velocity = Vector3.zero;
            }
            foreach (var ent in filterRbStop1)
            {
                pool.Rb.Get(ent).Value.velocity = Vector3.zero;
            }
        }
    }
}