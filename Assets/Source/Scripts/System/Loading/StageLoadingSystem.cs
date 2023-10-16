using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Movement;
using Source.Scripts.Data;
using Source.Scripts.Data.Enum;
using Source.Scripts.View;
using UnityEngine;

public class StageLoadingSystem : GameSystem
{
  
    public override void OnInit()
    {
        base.OnInit();
        
        var level = FindObjectOfType<StageView>();
        var views = level.gameObject.GetComponentsInChildren<BaseView>();
        foreach (var view in views)
        {
            game.Fabric.InitView((BaseView) view);
        }
    }
    
  
}
