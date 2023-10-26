using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle.Ball;
using UnityEngine;

namespace Source.Scripts.System.Battle.BallPerks
{
    public class BurnTickSystem : GameSystem
    {
        private EcsFilter filterBurnTick;
        public override void OnInit()
        {
            base.OnInit();
            filterBurnTick = world.Filter<BurnTick>().Inc<Fire>().Inc<Hp>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var ent in filterBurnTick)
            {
                ref var burnTick =ref pool.BurnTick.Get(ent);
              
                if (burnTick.Time<=0)
                {  
                    ref var fire =ref pool.Fire.Get(ent);
                    fire.BurnTime -= fire.BurnTick;
                    if (fire.BurnTime<=0)
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
    }
}