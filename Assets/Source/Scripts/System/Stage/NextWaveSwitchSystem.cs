using Ketchapp.MayoSDK;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle;


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
                if(stage.AllWavesComplete)
                    continue;
                
              
                if (stage.AllEnemiesDead)
                {
                    if (stage.CurrentWaveId<stage.Waves.Count-1)
                    {
                        //Next Wave
                        stage.CurrentWaveId++;
                        stage.CurrentWaveEnemiesSpawnedCount = 0;
                        pool.AnalyticsEvent.Add(eventWorld.NewEntity()).Value =
                            $"lvl{save.StageToLoad+1}_wave{stage.CurrentWaveId+1-1}_finish";
                       
                    }
                    else
                    { 
                        //Next Level
                        pool.NextStageEvent.Add(eventWorld.NewEntity());
                    }

                }
            }
        
        }
    }
}