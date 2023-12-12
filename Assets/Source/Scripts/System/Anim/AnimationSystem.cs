using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle.Tome;
using Source.Scripts.Component.Event;
using Source.Scripts.Component.Movement;
using Source.Scripts.Component.ViewComponent;
using UnityEngine;

namespace Source.Scripts.System.Anim
{
    public class AnimationSystem : GameSystem
    {
        private EcsFilter filterMove;
        private EcsFilter filterStopMove;
        private EcsFilter filterEvent;
        private EcsFilter filterStopMoveRb;
        private EcsFilter filterDead;
        private EcsFilter filterDeadTick;


        public override void OnInit()
        {
            base.OnInit();

            filterMove = world.Filter<AnimationComponent>().Inc<Speed>().Exc<CantMoveTag>().Exc<KnockedTick>().End();
            filterStopMove = world.Filter<AnimationComponent>().Inc<CantMoveTag>().End();
            filterStopMoveRb = world.Filter<AnimationComponent>().Inc<RigidbodyComponent>().Exc<Speed>().End();
            filterDead = world.Filter<AnimationComponent>().Inc<DeadTag>().Exc<DeathAnimTick>().End();
            filterDeadTick = world.Filter<AnimationComponent>().Inc<DeadTag>().Inc<DeathAnimTick>().End();
            
            filterEvent = eventWorld.Filter<DamageEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var ent in filterMove)
            {
                var animationView = pool.Anim.Get(ent).Value;
                animationView.Play("Run");
            }

            foreach (var ent in filterStopMove)
            {
                var animationView = pool.Anim.Get(ent).Value;
                animationView.Play("Idle");
            }

            foreach (var ent in filterStopMoveRb)
            {
                var animationView = pool.Anim.Get(ent).Value;
                animationView.Play("Idle");
            }

            foreach (var e in filterEvent)
            {
                var damageEvent = pool.DamageEvent.Get(e);
                if (damageEvent.Target == game.PlayerEntity)
                    continue;
                var animationView = pool.Anim.Get(damageEvent.Target).Value;
                //animationView.Bounce();
                animationView.Play("GetHit");
                //animationView.GetHitVFX();
            }

            foreach (var ent in filterDead)
            {
                var animationView = pool.Anim.Get(ent).Value;
                pool.DeathAnimTick.Add(ent).Value = animationView.DeathAnimLenght;
                pool.NavMeshAgentComponent.Get(ent).Value.enabled = false;
                animationView.AnimateDeath();
            }

            foreach (var ent in filterDeadTick)
            {
                ref var time = ref pool.DeathAnimTick.Get(ent).Value;
                if (time <= 0)
                {
                    pool.DeathAnimTick.Del(ent);
                }
                else 
                    time -= Time.deltaTime;
            }
        }
    }
}