using Ketchapp.MayoSDK;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Event;
using Source.Scripts.Data.Enum;
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
            KetchappSDK.Analytics.GetLevel((save.StageToLoad) + 1)
                .ProgressionStart();
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
                    KetchappSDK.Analytics.GetLevel((save.StageToLoad) + 1)
                        .ProgressionFailed();
                    
                    SetUI(false);
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
                KetchappSDK.Analytics.GetLevel((save.StageToLoad) + 1-1)
                    .ProgressionComplete();

                SetUI(true);
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


        private void SetUI(bool isWin)
        {
            var stage = pool.Stage.Get(game.StageEntity);
            int wavesCount;
            if (isWin)
                wavesCount = stage.Waves.Count;
            else
                wavesCount = stage.CurrentWaveId;
            
            var reward = (int) (stage.StartReward + wavesCount * stage.RewardPerWave);
            save.PlayerInventory[ResType.GOLD] += reward;
            //stage is already incremented
            if (isWin)
            {
                winUIScreen.SetStageNumber(save.StageToLoad+1-1);
                winUIScreen.SetKills(game.KillsCount);
                winUIScreen.SetReward(reward);
                winUIScreen.SetWaves(wavesCount);
            }
            else
            {
                loseUIScreen.SetKills(game.KillsCount);
                loseUIScreen.SetReward(reward);
                loseUIScreen.SetWaves(wavesCount);
            }
               
            Time.timeScale = 0;
        }

       

    }
}