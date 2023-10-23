using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle;
using UnityEngine;

namespace Source.Scripts.System.Battle
{
    public class WaveDelayTickSystem : GameSystem
    {
        private EcsFilter filterAddTick;
        private EcsFilter filterTick;
        
        public override void OnInit()
        {
            base.OnInit();
            filterAddTick = world.Filter<Stage>().Exc<WaveDelayTick>().Exc<EnemySpawnTick>().End();
            filterTick = world.Filter<Stage>().Inc<WaveDelayTick>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var ent in filterAddTick)
            {
                var stage = pool.Stage.Get(ent);
                if (stage.CurrentWave<stage.Waves.Count)
                {
                    pool.WaveDelayTick.Add(ent).Value = stage.Waves[stage.CurrentWave].WaveDelay;
                }
            }

            foreach (var ent in filterTick)
            {
                ref var tick =ref pool.WaveDelayTick.Get(ent);
                if (tick.Value<=0)
                {
                    pool.WaveDelayTick.Del(ent);
                    StartWave(ent);
                }
                else
                {
                    tick.Value -= Time.deltaTime;
                }
            }
        }

        private void StartWave(int ent)
        {
            
        }
        
     
    }
}