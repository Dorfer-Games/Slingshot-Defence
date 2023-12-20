using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Event;
using Source.Scripts.Component.ViewComponent;
using Source.Scripts.Data.Enum;
using UnityEngine;

namespace Source.Scripts.System.Ui
{
   public class WorldUpdateUISystem : GameSystem
   {
       private EcsFilter filterPlayerDataChanged;
        private EcsFilter filterHpView;
        private EcsFilter filterStorageCounter;
        private EcsFilter filterDamageEvent;
        private EcsFilter filterResGet;
        private EcsFilter filterPlayerResGet;

        public override void OnInit()
        {
            base.OnInit();
            
            filterHpView = world.Filter<HpViewComponent>().Inc<Hp>().End();
            filterDamageEvent = eventWorld.Filter<DamageEvent>().End();
            UpdateUI();
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            UpdateUI();
        }

        private void UpdateUI()
        {
            foreach (var ent in filterHpView)
            {
                var hp = pool.Hp.Get(ent);
                var hpView = pool.HpViewComponent.Get(ent).Value;
                if (Mathf.Abs(hp.CurHp - hp.MaxHp) < 0.01f || pool.Dead.Has(ent))
                {
                    hpView.ToggleActive(false);
                    hpView.SetValue(1);
                }
                else
                {
                    hpView.ToggleActive(true);
                    hpView.SetValue(hp.CurHp/hp.MaxHp);
                }

            }
            
            foreach (var e in filterDamageEvent)
            {
                var damageEvent = pool.DamageEvent.Get(e);
                var target = damageEvent.Target;
                if (pool.HpViewComponent.Has(target))
                {
                    var hpBarUIView = pool.HpViewComponent.Get(target).Value;
                    var link = pool.View.Get(target).Value.gameObject;
                    hpBarUIView.AnimateDamageFadeNumber(Mathf.CeilToInt(damageEvent.Damage),link);

                    if (pool.Dead.Has(target))
                    {
                        hpBarUIView.AnimateGoldFadeNumber(pool.Inventory.Get(target).Value[ResType.GOLD],link);
                    }
                } 
                
            }
            
        }
    }
}