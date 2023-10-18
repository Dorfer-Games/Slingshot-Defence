using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Movement;
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
        
        game.PlayerEntity = game.Fabric.InitView(playerView);
        game.CameraSwitcherView = cameraSwitcherView;
        game.Joystick = joystick;
        game.PlayerView = playerView;
        var ammoBalls = playerView.AmmoView.GetComponentsInChildren<BaseView>();
        foreach (var ent in ammoBalls)
        {
            game.Fabric.InitView(ent);
        }
    }
}
