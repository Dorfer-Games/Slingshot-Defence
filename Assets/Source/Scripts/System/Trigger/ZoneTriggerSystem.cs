using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle;
using Source.Scripts.Component.Event;
using Source.Scripts.View;
using UnityEngine;

namespace Source.Scripts.System.Trigger
{
    public class ZoneTriggerSystem : GameSystem
    {
        private EcsFilter filterSpawnZoneEvent;
       

        public override void OnInit()
        {
            base.OnInit();
            filterSpawnZoneEvent = eventWorld.Filter<SpawnZoneEvent>().End();
           
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filterSpawnZoneEvent)
            {
                var ent = pool.SpawnZoneEvent.Get(e).Entity;
                SubscribeZone(ent);
                EnterOnInit(ent);

                pool.SpawnZoneEvent.Del(e);
            }
            
           
        }

        private void EnterOnInit(int ent)
        {
            //check collisions when init
            var zoneT = pool.View.Get(ent).Value.transform;
            var radius = pool.Zone.Get(ent).Radius;
            var colliders = Physics.OverlapSphere(zoneT.position, radius, config.EnemyLayerMask);
            foreach (var c in colliders)
                ZoneEnter(zoneT, c.transform);
        }

        private void SubscribeZone(int ent)
        {
            var baseView = pool.View.Get(ent).Value;
            var triggerListener = baseView.GetComponentInChildren<TriggerListenerView>();
            triggerListener.OnTriggerEnterEvent += ZoneEnter;
            triggerListener.OnTriggerExitEvent += ZoneExit;
        }

        private void ZoneEnter(Transform sender, Transform other)
        {
            var otherE = other.GetComponentInParent<BaseView>().Entity;
            var senderE = sender.GetComponentInParent<BaseView>().Entity;

            ref var zoneEnterEvent = ref pool.ZoneEnterEvent.Add(eventWorld.NewEntity());
            zoneEnterEvent.Sender = senderE;
            zoneEnterEvent.Target = otherE;
        }

        private void ZoneExit(Transform sender, Transform other)
        {
            var otherE = other.GetComponentInParent<BaseView>().Entity;
            var senderE = sender.GetComponentInParent<BaseView>().Entity;

            ref var zoneExitEvent = ref pool.ZoneExitEvent.Add(eventWorld.NewEntity());
            zoneExitEvent.Sender = senderE;
            zoneExitEvent.Target = otherE;
        }
    }
}