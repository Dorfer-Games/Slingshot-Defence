using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle;
using UnityEngine;

namespace Source.Scripts.System.Battle
{
    public class EnemySpawnTickSystem : GameSystem
    {
        private EcsFilter filterTick;

        public override void OnInit()
        {
            base.OnInit();
            filterTick = world.Filter<Stage>().Inc<EnemySpawnTick>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();


            foreach (var ent in filterTick)
            {
                ref var tick = ref pool.EnemySpawnTick.Get(ent);
                if (tick.Value <= 0)
                {
                    SpawnNextEnemy(ent);

                    ChangeTick(ent);
                }
                else
                {
                    tick.Value -= Time.deltaTime;
                }
            }
        }

        private void ChangeTick(int ent)
        {
            ref var stage = ref pool.Stage.Get(ent);
            if (!stage.AllEnemiesSpawned)
            {
                var enemy = stage.CurrentWave.Enemies[stage.CurrentWaveEnemiesSpawnedCount];
                if (stage.UseCommonSpawnDelay)
                {
                    pool.EnemySpawnTick.Get(ent).Value = stage.CommonSpawnDelay;
                }
                else
                    pool.EnemySpawnTick.Get(ent).Value =
                        config.EnemyConfigs[enemy.EnemyType].LevelStats[enemy.Level].SpawnDelay;
            }
            else
            {
                pool.EnemySpawnTick.Del(ent);
            }
        }

        private void SpawnNextEnemy(int ent)
        {
            ref var stage = ref pool.Stage.Get(ent);
            var enemy = stage.CurrentWave.Enemies[stage.CurrentWaveEnemiesSpawnedCount];
            var baseView = Instantiate(config.EnemyConfigs[enemy.EnemyType].Prefab);
            baseView.transform.position = stage.SpawnPos.position;
            baseView.transform.rotation = Quaternion.Euler(0, 180, 0);
            var enemyEnt = game.Fabric.InitView(baseView);
            var stats = config.EnemyConfigs[enemy.EnemyType].LevelStats[enemy.Level];

            //add other stats
            pool.Enemy.Add(enemyEnt);
            ref var hp =ref pool.Hp.Add(enemyEnt);
            hp.CurHp = hp.MaxHp = stats.Hp;
            pool.Exp.Add(enemyEnt).Value = stats.Exp;
            pool.Inventory.Add(enemyEnt);
            pool.Speed.Add(enemyEnt).Value = stats.Speed;
            pool.NavMeshAgentComponent.Get(enemyEnt).Value.destination = game.PlayerView.transform.position;

            stage.AliveEnemies.Add(enemyEnt);
            stage.CurrentWaveEnemiesSpawnedCount++;
        }
    }
}