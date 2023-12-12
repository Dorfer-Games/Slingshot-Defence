using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle;
using Source.Scripts.Data.Enum;
using UnityEngine;

namespace Source.Scripts.System.Battle
{
    public class WeaknessTickSystem : GameSystem
    {
        private EcsFilter filterTick;
        public override void OnInit()
        {
            base.OnInit();
            filterTick = world.Filter<Weakness>().Inc<Hp>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var ent in filterTick)
            {
               ref var weakness = ref pool.Weakness.Get(ent);
               if (weakness.Time<0)
               {
                   pool.Weakness.Del(ent);
                   
                   ref var vfxEvent = ref pool.VFXEvent.Add(eventWorld.NewEntity());
                   vfxEvent.VFXId = (int)ElementType.DARKNESS+config.ElementsCount-1;
                   vfxEvent.Target = ent;
                   vfxEvent.Toggle = false;
               }
               else
                   weakness.Time -= Time.deltaTime;
            }
        }
    }
}