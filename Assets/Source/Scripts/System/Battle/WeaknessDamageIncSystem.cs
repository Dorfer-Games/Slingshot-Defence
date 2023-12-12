using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle;
using Source.Scripts.Component.Event;
using UnityEngine;

namespace Source.Scripts.System.Battle
{
    public class WeaknessDamageIncSystem : GameSystem
    {
        private EcsFilter filterDamage;
        public override void OnInit()
        {
            base.OnInit();
            filterDamage = eventWorld.Filter<DamageEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filterDamage)
            {
                ref var damageEvent = ref pool.DamageEvent.Get(e);
                var target = damageEvent.Target;
                ref var damage = ref damageEvent.Damage;
                if (pool.Weakness.Has(target))
                {
                    var dmgIncPercent = pool.Weakness.Get(target).AddDamageIncomePercent;
                    damage += damage * dmgIncPercent / 100f;
                }
              
            }
        }
    }
}