using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.Event;
using UnityEngine;

namespace Source.Scripts.System
{
    public class DeleteEventSystem : GameSystem
    {
        private Dictionary<EcsFilter,IEcsPool> filtersPools;

        public override void OnInit()
        {
            base.OnInit(); 
            
            filtersPools = new ();
         
            AddFilter<HitEvent>();
            AddFilter<ShotEvent>();
            AddFilter<ShotCancelEvent>();
            AddFilter<SpawnBallEvent>();
            AddFilter<DamageEvent>();
            AddFilter<HitPlayerEvent>();
            AddFilter<SetOnFireEvent>();
            AddFilter<ZoneEnterEvent>();
            AddFilter<ZoneExitEvent>();
        
        }

        private void AddFilter<T>() where T: struct
        {
            filtersPools.Add(eventWorld.Filter<T>().End(),eventWorld.GetPool<T>());
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var filter in filtersPools.Keys)
            {
                foreach (var ent in filter)
                {
                    filtersPools[filter].Del(ent);
                }
            }
           

        }
    }
}