using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle;
using Source.Scripts.Component.Event;
using UnityEngine;

namespace Source.Scripts.System.Battle
{
    public class ZoneSystem : GameSystem
    {
        private EcsFilter filterZoneEnter;
        private EcsFilter filterZoneExit;
        private EcsFilter filterZoneTick;
        private EcsFilter filterZoneTriggerers;

        public override void OnInit()
        {
            base.OnInit();
            filterZoneEnter = eventWorld.Filter<ZoneEnterEvent>().End();
            filterZoneExit = eventWorld.Filter<ZoneExitEvent>().End();
            filterZoneTick = world.Filter<ZoneTick>().End();
            filterZoneTriggerers = world.Filter<ZoneTriggers>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filterZoneEnter)
            {
                var zoneEnterEvent = pool.ZoneEnterEvent.Get(e);
                var sender = zoneEnterEvent.Sender;
                var target = zoneEnterEvent.Target;
                ZoneEnter(sender, target);
            }

            foreach (var e in filterZoneExit)
            {
                var zoneExitEvent = pool.ZoneExitEvent.Get(e);
                var sender = zoneExitEvent.Sender;
                var target = zoneExitEvent.Target;
                ZoneExit(sender, target);
            }

            foreach (var ent in filterZoneTick)
            {
                ref var time = ref pool.ZoneTick.Get(ent).Value;
                if (time <= 0)
                {
                    foreach (var target in filterZoneTriggerers)
                    {
                        ZoneExit(ent, target);
                    }
                    pool.Dead.Add(ent);
                }
                else
                    time -= Time.deltaTime;
            }
        }

        private void ZoneEnter(int sender, int target)
        {
            pool.ZoneTriggers.Get(target).Value.Add(sender);
            if (pool.Slime.Has(sender))
            {
                ApplySlow(sender, target);
            }
            else if (pool.Fire.Has(sender))
            {
                ref var setOnFireEvent = ref pool.SetOnFireEvent.Add(eventWorld.NewEntity());
                setOnFireEvent.Sender = sender;
                setOnFireEvent.Target = target;
            }
        }

        private void ZoneExit(int sender, int other)
        {
            pool.ZoneTriggers.Get(other).Value.Remove(sender);
            if (pool.Slime.Has(sender))
            {
                GiveSpeed(sender, other);
            }
        }

        private void ApplySlow(int senderE, int otherE)
        {
            var slowPercent = pool.Slime.Get(senderE).SlowPercent;
            ref var speed = ref pool.Speed.Get(otherE).Value;
            var maxSpeed = pool.MaxSpeed.Get(otherE).Value;
            speed =maxSpeed- maxSpeed * slowPercent / 100f;
            if (speed < 0)
                speed = 0;
        }

        private void GiveSpeed(int senderE, int otherE)
        {
            var zones = pool.ZoneTriggers.Get(otherE).Value;
            bool isLastZone = true;
            foreach (var zoneE in zones)
            {
                if (pool.Slime.Has(zoneE))
                {
                    isLastZone = false;
                    break;
                }
            }

            //enemy is in only 1 zone
            if (isLastZone)
            {
                pool.Speed.Get(otherE).Value = pool.MaxSpeed.Get(otherE).Value;
            }
        }
    }
}