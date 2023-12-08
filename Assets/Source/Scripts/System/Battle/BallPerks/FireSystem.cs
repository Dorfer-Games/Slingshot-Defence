using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle;
using Source.Scripts.Component.Battle.Ball;
using Source.Scripts.Component.Event;
using UnityEngine;

namespace Source.Scripts.System.Battle.BallPerks
{
    public class FireSystem : GameSystem
    {
        private EcsFilter filterBurnTick;
        private EcsFilter filterSetOnFire;

        public override void OnInit()
        {
            base.OnInit();
            filterBurnTick = world.Filter<BurnTick>().Inc<Fire>().Inc<Hp>().Inc<ZoneTriggers>().End();
            filterSetOnFire = eventWorld.Filter<SetOnFireEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var e in filterSetOnFire)
            {
                var setOnFireEvent = pool.SetOnFireEvent.Get(e);
                var sender = setOnFireEvent.Sender;
                var target = setOnFireEvent.Target;
                var damage = setOnFireEvent.Damage;

                ref var fire = ref pool.Fire.GetOrCreateRef(target);
                fire = pool.Fire.Get(sender);

                ref var burnTick = ref pool.BurnTick.GetOrCreateRef(target);
                burnTick.Damage = damage * fire.BurnTickDamagePercent / 100f;
                burnTick.Time = fire.BurnTick;
            }

            foreach (var ent in filterBurnTick)
            {
                ref var burnTick = ref pool.BurnTick.Get(ent);

                if (burnTick.Time <= 0)
                {
                    ref var fire = ref pool.Fire.Get(ent);
                    if (!IsInFireZone(ent))
                        fire.BurnTime -= fire.BurnTick;
                   
                    if (fire.BurnTime <= 0)
                    {
                        pool.BurnTick.Del(ent);
                        pool.Fire.Del(ent);
                    }
                    else
                    {
                        burnTick.Time = fire.BurnTick;
                    }

                    ref var damageEvent = ref pool.DamageEvent.Add(eventWorld.NewEntity());
                    damageEvent.Sender = ent;
                    damageEvent.Target = ent;
                    damageEvent.Damage = burnTick.Damage;
                }
                else
                {
                    burnTick.Time -= Time.deltaTime;
                }
            }
        }

        private bool IsInFireZone(int ent)
        {
            var zones = pool.ZoneTriggers.Get(ent).Value;
            foreach (var zoneE in zones)
            {
                if (pool.Fire.Has(zoneE))
                    return true;
            }

            return false;
        }
    }
}