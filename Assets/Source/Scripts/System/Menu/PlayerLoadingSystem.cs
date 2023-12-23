using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ketchapp.MayoSDK;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Movement;
using Source.Scripts.Data;
using Source.Scripts.Data.Enum;
using Source.Scripts.View;
using Source.Scripts.View.Cam;
using UnityEngine;

public class PlayerLoadingSystem : GameSystem
{
    public override void OnInit()
    {
        base.OnInit();
        var playerView = FindObjectOfType<PlayerView>();
        var cameraSwitcherView = FindObjectOfType<CameraSwitcherView>();
        var joystick = FindObjectOfType<Joystick>();

        save.Slings ??= new() {SlingType.DEFAULT};
        save.AnalyticsProgress ??= new AnalyticsProgress();
        save.UnlockedElements ??= new ();
        save.UnlockedTomes ??= new ();
        save.UnlockedUlts ??= new ();
        if (!save.AnalyticsProgress.IsLoggedTutorialCompleted)
        {
            save.AnalyticsProgress = new AnalyticsProgress();
        }

        LogFirstGameLaunch();
        
        
        game.PlayerEntity = game.Fabric.InitView(playerView);
        game.CameraSwitcherView = cameraSwitcherView;
        game.Joystick = joystick;
        game.PlayerView = playerView;

        pool.Tomes.Add(game.PlayerEntity);
        pool.Elements.Add(game.PlayerEntity);
        
        //ammo init
        var ammoBalls = playerView.AmmoView.AmmoBalls;
        var list = new List<int>();
        foreach (var view in ammoBalls)
        {
            var ent = game.Fabric.InitView(view);
            list.Add(ent);
        }

        ref var ammo = ref pool.Ammo.Add(game.PlayerEntity);
        ammo.Value = list;
        ammo.Count = list.Count;
    }
    
    private void LogFirstGameLaunch()
    {
        if (!save.AnalyticsProgress.IsLoggedFirstLaunched)
        {
            KetchappSDK.Analytics.CustomEvent("first_game_launch");
            save.AnalyticsProgress.IsLoggedFirstLaunched = true;
            Bootstrap.Instance.SaveGame();
        }
    }
}
