using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using NaughtyAttributes;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle;
using Source.Scripts.Component.Event;
using Source.Scripts.Component.Movement;
using Source.Scripts.View;
using UnityEngine;

namespace Source.Scripts.System.Trigger
{
    public class TriggerSystem : GameSystem
    {
        [SerializeField] [Tag] private string attackable;
        
        private EcsFilter filterSpawnBallEvent;
        private EcsFilter filterBallReachEvent;


        public override void OnInit()
        {
            base.OnInit();
            filterSpawnBallEvent = eventWorld.Filter<SpawnBallEvent>().End();
            filterBallReachEvent = eventWorld.Filter<ReachEvent>().End();

            //sub player
            game.PlayerView.BodyTriggerListener.OnTriggerEnterEvent += PlayerHit;
        }

        private void PlayerHit(Transform sender,Transform other)
        {
            if (!other.tag.Equals(attackable))
                return;
         
            var otherE = other.GetComponentInParent<BaseView>().Entity;

            ref var hitEvent = ref pool.HitPlayerEvent.Add(eventWorld.NewEntity());
            hitEvent.Sender = otherE;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filterSpawnBallEvent)
            {
                var ent = pool.SpawnBallEvent.Get(e).Value;
                SubscribeBall(ent);
            }
            foreach (var e in filterBallReachEvent)
            {
                var ent = pool.ReachEvent.Get(e).Entity;
                if (pool.Enemy.Has(ent))
                    continue;
                
                Hit(ent);
            }
        }

        private void SubscribeBall(int ent)
        {
            var baseView = pool.View.Get(ent).Value;
            var damagerView = baseView.GetComponent<DamagerView>();
            damagerView.AttackTriggerListener.OnTriggerEnterEvent += TriggerHit;
        }
        

        private void TriggerHit(Transform sender,Transform other)
        {
            if (!other.tag.Equals(attackable))
                return;
         
            var senderE = sender.GetComponentInParent<BaseView>().Entity;
            Hit(senderE);
        }

        private void Hit(int senderE)
        {
            var hitRadius = pool.Radius.Get(senderE).Value;
            var targets = game.PositionService.GetEnemiesInRadius(senderE, hitRadius);
            
            var vfx = Instantiate(config.BaseHitVFXPrefab);
            vfx.transform.position = pool.View.Get(senderE).Value.transform.position;
            //hit only obstacle
            if (targets.Count==0)
            {
                /*var otherE = other.GetComponentInParent<BaseView>().Entity;
                if (pool.Obstacle.Has(otherE))
                {
                    pool.Dead.GetOrCreateRef(senderE);
                    return;
                }*/
                pool.Dead.GetOrCreateRef(senderE);
                return;
            }

            ref var hitEventComponent = ref pool.HitEvent.Add(eventWorld.NewEntity());
            hitEventComponent.Targets = targets;
            hitEventComponent.Sender = senderE;
        }
    }
}