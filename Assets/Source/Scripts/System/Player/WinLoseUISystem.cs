using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Event;
using Source.Scripts.View;
using UnityEngine;

namespace Source.Scripts.System.Player
{
    public class WinLoseUISystem : GameSystem
    {
        private EcsFilter filterDead;
        private EcsFilter filterNextLevel;
        private WinUIScreen winUIScreen;
        private LoseUIScreen loseUIScreen;
        
        public override void OnInit()
        {
            base.OnInit();
            winUIScreen = FindObjectOfType<WinUIScreen>(true);
            loseUIScreen = FindObjectOfType<LoseUIScreen>(true);
            
            filterDead = world.Filter<DeadTag>().End();
            filterNextLevel = eventWorld.Filter<NextStageEvent>().End();
        }
        

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var ent in filterDead)
            {
                if (ent==game.PlayerEntity)
                {
                    pool.Dead.Del(ent);
                    if (game.IsFinished)
                        break;
                    game.IsFinished = true;
                    
                    SetUILose();
                    loseUIScreen.MenuButton.onClick.AddListener(() =>
                    {
                        save.SkipMenu = false;
                        Time.timeScale = 1;
                        Bootstrap.Instance.SaveGame();
                        Bootstrap.Instance.GameRestart(0);
                    });
                    loseUIScreen.Open();
                }
            }

            foreach (var e in filterNextLevel)
            {
                if (game.IsFinished)
                    break;
                game.IsFinished = true;
                
                save.StageToLoad++;
                if (save.StageToLoad==config.MaxLevels)
                    save.StageToLoad--;

                SetUIWin();
                winUIScreen.NextStageButton.onClick.AddListener(() =>
                {
                    save.SkipMenu = true;
                    Time.timeScale = 1;
                    Bootstrap.Instance.SaveGame();
                    Bootstrap.Instance.GameRestart(0);
                });
                winUIScreen.Open();
            }
        }
        

        private void SetUIWin()
        {
            var stage = pool.Stage.Get(game.StageEntity);
            var wavesCount = stage.Waves.Count;
            //stage is already incremented
            winUIScreen.SetStageNumber(save.StageToLoad+1-1);
            winUIScreen.SetKills(game.KillsCount);
            winUIScreen.SetReward((int) (stage.StartReward+wavesCount*stage.RewardPerWave));
            winUIScreen.SetWaves(wavesCount);
            Time.timeScale = 0;
        }
        private void SetUILose()
        {  
            var stage = pool.Stage.Get(game.StageEntity);
            var wavesCount = stage.CurrentWaveId;
            //loseUIScreen.SetStageNumber(save.StageToLoad+1);
            loseUIScreen.SetKills(game.KillsCount);
            loseUIScreen.SetReward((int) (stage.StartReward+wavesCount*stage.RewardPerWave));
            loseUIScreen.SetWaves(wavesCount);
            Time.timeScale = 0;
        }


    }
}