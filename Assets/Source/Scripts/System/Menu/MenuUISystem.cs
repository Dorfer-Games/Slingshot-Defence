
using Ketchapp.MayoSDK;
using Kuhpik;

using Source.Scripts.Data.Enum;


public class MenuUISystem : GameSystemWithScreen<MenuUIScreen>
{
    public override void OnInit()
    {
        base.OnInit();
        screen.SelectItem(screen.WorldItem);
        screen.LibItem.Button.interactable = save.AnalyticsProgress.IsLoggedTutorialCompleted;
        screen.UpsItem.Button.interactable = save.AnalyticsProgress.IsLoggedTutorialCompleted;
        
        SetWorldStagesUI();
        SetUnlockedCards();
        
        SubscribeSlingUpsUI();
        SetSlingUpsUI();
        screen.SetGold(save.PlayerInventory[ResType.GOLD]);
    }

    private void SetUnlockedCards()
    {
        foreach (var elementType in save.UnlockedElements)
            screen.LockedElements[elementType].gameObject.SetActive(false);
        foreach (var elementType in save.UnlockedUlts)
            screen.LockedUlts[elementType].gameObject.SetActive(false);
        foreach (var tomeType in save.UnlockedTomes)
            screen.LockedTomes[tomeType].gameObject.SetActive(false);
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
            case UpType.CRIT_K:
                name = "crit";
                break;
        }
        KetchappSDK.Analytics.CustomEvent($"upgrade_{name}{level}");
    }

    private void SetSlingUpsUI()
    {
        var ups = save.SlingUps[save.CurrentSling];
        foreach (var typeLevel in ups)
            SetSlingUpCost(typeLevel.Key);
    }

    private void SetSlingUpCost(UpType upType)
    {
        var upsLevels = save.SlingUps[save.CurrentSling];
        var upsCosts = config.SlingConfigs[save.CurrentSling].UpCosts;
        var ups = config.SlingConfigs[save.CurrentSling].Ups;
        var upLevel = upsLevels[upType];
        var screenSlingUp = screen.SlingUps[upType];
        if (upLevel+1<upsCosts[upType].Length)
        {
            screenSlingUp.SetCost(upsCosts[upType][upLevel+1],save.PlayerInventory[ResType.GOLD]);
            screenSlingUp.SetValues(ups[upType][upLevel],ups[upType][upLevel+1]);
        }
        else
        {
            screenSlingUp.SetMax();
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