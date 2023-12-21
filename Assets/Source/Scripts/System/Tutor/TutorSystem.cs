using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Data.Enum;
using Source.Scripts.UI.Screen;
using UnityEngine;

namespace Source.Scripts.System.Tutor
{
    public class TutorSystem : GameSystemWithScreen<TutorUIScreen>
    {
        private EcsFilter filterEnemy;
        
        public override void OnInit()
        {
            base.OnInit();
            filterEnemy = world.Filter<Enemy>().End();
            TrySendTutorStart();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (save.AnalyticsProgress.IsLoggedTutorialCompleted)
                return;
            var curStep = save.AnalyticsProgress.TutorStepType;
            if (curStep==TutorStepType.FIRST_ENEMY_RUN)
            {
                var stage = pool.Stage.Get(game.StageEntity);
                
                //first enemy spawned
                if (stage.CurrentWaveEnemiesSpawnedCount==1 && stage.CurrentWaveId==0)
                {
                    //set exp to next lvl
                    foreach (var ent in filterEnemy)
                    {
                        pool.Exp.Get(ent).Value = config.ExpProgression[1];

                        if (pool.View.Get(ent).Value.transform.position.z<25)
                            NextStep();
                        break;
                    }
                }
            }

            if (curStep==TutorStepType.SHOW_CONTROLS)
            {
                Time.timeScale = 0;
                game.Joystick.transform.SetAsLastSibling();
                screen.ToggleControlsScreen(true);
                if (UnityEngine.Input.GetMouseButton(0))
                {
                    Time.timeScale = config.SlowTimeScale;
                    game.Joystick.transform.SetAsFirstSibling();
                    screen.ToggleControlsScreen(false);
                    NextStep();
                }
            }
            
            if (curStep==TutorStepType.SHOW_AMMO)
            {
                Time.timeScale = 0;
                game.Joystick.transform.SetAsLastSibling();
                screen.ToggleAmmoScreen(true);
                if (UnityEngine.Input.GetMouseButton(0))
                {
                    Time.timeScale = config.SlowTimeScale;
                    game.Joystick.transform.SetAsFirstSibling();
                    screen.ToggleAmmoScreen(false);
                    NextStep();
                }
            }
        }

        private void NextStep()
        {
            SendTutorEventComplete();
            save.AnalyticsProgress.TutorStepType++;
            TrySendTutorEnd();
        }

        private void SendTutorEventComplete()
        {
            pool.AnalyticsEvent.Add(eventWorld.NewEntity()).Value =
                $"tutorial_step{(int)save.AnalyticsProgress.TutorStepType+1}_{save.AnalyticsProgress.TutorStepType.ToString().ToLower()}";
        }
        
        private void TrySendTutorStart()
        {
            if (!save.AnalyticsProgress.IsLoggedTutorialStarted)
            {
                pool.AnalyticsEvent.Add(eventWorld.NewEntity()).Value =
                    $"tutorial_started";
                save.AnalyticsProgress.IsLoggedTutorialStarted = true;
            }
        }
        private void TrySendTutorEnd()
        {
            if (!save.AnalyticsProgress.IsLoggedTutorialCompleted && save.AnalyticsProgress.TutorStepType==TutorStepType.COMPLETE)
            {
                pool.AnalyticsEvent.Add(eventWorld.NewEntity()).Value =
                    $"tutorial_completed";
                save.AnalyticsProgress.IsLoggedTutorialCompleted = true;
            }
        }
    }
}