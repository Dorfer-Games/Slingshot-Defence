using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using UnityEngine;

namespace Source.Scripts.System.Battle
{
    public class ReloadTickSystem : GameSystem
    {
        private EcsFilter filterAddTick;
        private EcsFilter filterTick;
        
        public override void OnInit()
        {
            base.OnInit();
            filterAddTick = world.Filter<Ammo>().Exc<ReloadTick>().End();
            filterTick = world.Filter<Ammo>().Inc<ReloadTick>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var ent in filterAddTick)
            {
                var count = pool.Ammo.Get(ent).Count;
                if (count==0)
                {
                    pool.ReloadTick.Add(ent).Value = config.ReloadTime;
                }
            }

            foreach (var ent in filterTick)
            {
                ref var reloadTick =ref pool.ReloadTick.Get(ent);
                if (reloadTick.Value<=0)
                {
                    pool.ReloadTick.Del(ent);
                    Reload(ent);
                }
                else
                {
                    reloadTick.Value -= Time.deltaTime;
                }
            }
        }

        private void Reload(int ent)
        {
            ref var ammo = ref pool.Ammo.Get(ent);
            ammo.Count = ammo.Value.Count;
            var firstBallPos = game.PlayerView.AmmoView.FirstBallPos;
            var offset = 0.5f;
            Shuffle(ammo.Value);
            float yRad = 1;
            for (int i = 0; i < ammo.Count; i++)
            {
                var baseView = pool.View.Get(ammo.Value[i]).Value;
                baseView.transform.position = new Vector3(0, i * yRad + offset, 0) + firstBallPos.position;
                baseView.ToggleActive(true);
            }
        }
        
        private void Shuffle(List<int> list)  
        {
            int n = list.Count;  
            while (n > 1) {  
                n--;
                int k = Random.Range(0, n + 1);  
                int value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
    }
}