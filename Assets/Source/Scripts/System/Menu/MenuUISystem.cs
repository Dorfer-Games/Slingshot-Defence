using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Movement;
using Source.Scripts.Data;
using Source.Scripts.Data.Enum;
using Source.Scripts.View;
using UnityEngine;

public class MenuUISystem : GameSystemWithScreen<MenuUIScreen>
{
    public override void OnInit()
    {
        base.OnInit();
        screen.SelectItem(screen.WorldItem);
        
        SetWorldStagesUI();
        
        SubscribeSlingUpsUI();
        SetSlingUpsUI();
        screen.SetGold(save.PlayerInventory[ResType.GOLD]);
    }

    private void SubscribeSlingUpsUI()
    {
        var ups = save.SlingUps[save.CurrentSling];
        foreach (var typeLevel in ups)
        {
            var slingUpView = screen.SlingUps[typeLevel.Key];
            slingUpView.Button.onClick.AddListener(() =>
            {
                var upsCosts = config.SlingConfigs[save.CurrentSling].UpCosts;
                var cost = upsCosts[typeLevel.Key][typeLevel.Value + 1];
                save.PlayerInventory[ResType.GOLD] -= cost;
                save.SlingUps[save.CurrentSling][typeLevel.Key]++;
                SetSlingUpsUI();
                screen.SetGold(save.PlayerInventory[ResType.GOLD]);

                SendAnalyticEvent(typeLevel.Key, save.SlingUps[save.CurrentSling][typeLevel.Key]);
            });
        }
    }

    private void SendAnalyticEvent(UpType upType,int level)
    {
        string name = "";
        switch (upType)
        {
            case UpType.DAMAGE:
                name = "damage";
                break;
            case UpType.RELOAD_TIME:
                name = "cooldown";
                break;
            case UpType.GOLD_K:
                name = "income";
                break;
        }
        pool.AnalyticsEvent.Add(eventWorld.NewEntity()).Value =
            $"upgrade_{name}{level}";
    }

    private void SetSlingUpsUI()
    {
        var ups = save.SlingUps[save.CurrentSling];
        foreach (var typeLevel in ups)
            SetSlingUpCost(typeLevel.Key);
    }

    private void SetSlingUpCost(UpType upType)
    {
        var ups = save.SlingUps[save.CurrentSling];
        var upsCosts = config.SlingConfigs[save.CurrentSling].UpCosts;
        var upLevel = ups[upType];
        if (upLevel+1<upsCosts[upType].Length)
        {
            screen.SlingUps[upType].SetCost(upsCosts[upType][upLevel+1],save.PlayerInventory[ResType.GOLD]);
        }
        else
        {
            screen.SlingUps[upType].SetMax();
        }
    }

    private void SetWorldStagesUI()
    {
        var curLvlId = save.StageToLoad + 1;
        var delta = curLvlId;
        foreach (var worldMenuData in config.WorldMenuDataConfig.WorldMenuDatas)
        {
            if (delta - worldMenuData.StagesCount <= 0)
            {
                screen.SetStagesText(delta, worldMenuData.StagesCount);
                screen.SetWorldText(worldMenuData.WorldName);
                break;
            }
            else
                delta -= worldMenuData.StagesCount;
        }
    }
}