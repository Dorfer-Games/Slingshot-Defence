using Kuhpik;
using Leopotam.EcsLite;
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


        public override void OnInit()
        {
            base.OnInit();

            filterMove = world.Filter<AnimatorComponent>().Inc<Speed>().Exc<CantMoveTag>().Exc<KnockedTick>().End();
            filterStopMove = world.Filter<AnimatorComponent>().Inc<CantMoveTag>().End();
            filterStopMoveRb = world.Filter<AnimatorComponent>().Inc<RigidbodyComponent>().Exc<Speed>().End();
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
        }
    }
}