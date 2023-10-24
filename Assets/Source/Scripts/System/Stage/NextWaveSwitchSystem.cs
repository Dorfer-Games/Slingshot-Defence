using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle;
using Source.Scripts.Component.Event;
using Source.Scripts.Component.Movement;
using Source.Scripts.Component.ViewComponent;
using UnityEngine;

namespace Source.Scripts.System.Move
{
    public class NextWaveSwitchSystem : GameSystem
    {
        private EcsFilter filter;
        public override void OnInit()
        {
            base.OnInit();
            filter = world.Filter<Stage>().Exc<WaveDelayTick>().Exc<EnemySpawnTick>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var ent in filter)
            {
                ref var stage = ref pool.Stage.Get(ent);
                if (stage.AllEnemiesDead)
                {
                    if (stage.CurrentWaveId<stage.Waves.Count)
                    {
                        //Next Wave
                        stage.CurrentWaveId++;
                        stage.CurrentWaveEnemiesSpawnedCount = 0;
                    }
                    else
                    {
                        //Next Level
                    }

                }
            }
        
        }
    }
}