using DG.Tweening;
using Kuhpik;
using Source.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinUIScreen : UIScreen
{
    [SerializeField] private CounterUIView rewardCounter;
    [SerializeField] private CounterUIView killsCounter;
    [SerializeField] private CounterUIView wavesCounter;
    [SerializeField] private TextMeshProUGUI stageNumberText;
    public Button NextStageButton;

    public void SetReward(int gold)
    {
        rewardCounter.SetText("+"+gold);
    }
    public void SetKills(int kills)
    {
        killsCounter.SetText(kills);
    }
    public void SetWaves(int waves)
    {
        wavesCounter.SetText(waves);
    }
    public void SetStageNumber(int stage)
    {
        stageNumberText.text=$"Stage {stage}";
    }
    
}