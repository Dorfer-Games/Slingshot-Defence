using Kuhpik;
using Leopotam.EcsLite;
using MoreMountains.NiceVibrations;
using Source.Scripts.Component.Event;

namespace Source.Scripts.System.Util
{
    public class VibrationSystem : GameSystem
    {
        private EcsFilter filterDamage;
        private EcsFilter filterDamageToPlayer;
        
        public override void OnInit()
        {
            base.OnInit();
            filterDamage = eventWorld.Filter<DamageEvent>().End();
            //filterDamageToPlayer = eventWorld.Filter<HitPlayerEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filterDamage)
            {
                Vibrate();
                break;
            }
        }
        
        private void Vibrate()
        {
            if (!save.VibroOn)
                return;

            MMVibrationManager.Vibrate();
          
        }
    }
}