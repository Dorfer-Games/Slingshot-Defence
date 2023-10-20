using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using NaughtyAttributes;
using Source.Scripts.Component;
using Source.Scripts.Component.Event;
using Source.Scripts.View;
using UnityEngine;

namespace Source.Scripts.System.Trigger
{
    public class TriggerSystem : GameSystem
    {
        [SerializeField] [Tag] private string attackable;


        private EcsFilter filterSpawnBallEvent;
        private EcsFilter filterEnemy;


        public override void OnInit()
        {
            base.OnInit();
            filterSpawnBallEvent = eventWorld.Filter<SpawnBallEvent>().End();
            filterEnemy = world.Filter<Enemy>().End();
            //sub player
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filterSpawnBallEvent)
            {
                var ent = pool.SpawnBallEvent.Get(e).Value;
                Subscribe(ent);
            }
        }

        private void Subscribe(int ent)
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
            var hitRadius = pool.Radius.Get(senderE).Value;
            var targets = game.PositionService.GetEntInRadius(senderE, filterEnemy, hitRadius);
            
            ref var hitEventComponent = ref pool.HitEvent.Add(eventWorld.NewEntity());
            hitEventComponent.Targets = targets;
            hitEventComponent.Sender = senderE;
            
        }
    }
}