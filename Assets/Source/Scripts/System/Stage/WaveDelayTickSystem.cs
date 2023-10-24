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
                if (stage.CurrentWaveId < stage.Waves.Count && stage.AliveEnemies.Count == 0)
                {
                    pool.WaveDelayTick.Add(ent).Value = stage.Waves[stage.CurrentWaveId].WaveDelay;
                }
            }

            foreach (var ent in filterTick)
            {
                ref var tick = ref pool.WaveDelayTick.Get(ent);
                if (tick.Value <= 0)
                {
                    pool.WaveDelayTick.Del(ent);
                    var stage = pool.Stage.Get(ent);
                    if (!stage.AllWavesComplete)
                    {
                        StartWave(ent);
                    }
                }
                else
                {
                    tick.Value -= Time.deltaTime;
                }
            }
        }

        private void StartWave(int ent)
        {
            var stage = pool.Stage.Get(ent);
            var enemy = stage.CurrentWave.Enemies[0];
            
            if (stage.UseCommonSpawnDelay)
                pool.EnemySpawnTick.Add(ent).Value = stage.CommonSpawnDelay;
            else
                pool.EnemySpawnTick.Add(ent).Value =
                    config.EnemyConfigs[enemy.EnemyType].LevelStats[enemy.Level].SpawnDelay;
        }
    }
}