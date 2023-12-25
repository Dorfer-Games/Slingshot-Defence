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
using UnityEngine.AI;

public class StageInitSystem : GameSystem
{
  
    public override void OnInit()
    {
        base.OnInit();
        
        var stage = FindObjectOfType<StageView>();
        var views = stage.gameObject.GetComponentsInChildren<BaseView>().ToList();
        game.StageEntity=  game.Fabric.InitView(views[0]);
        views.RemoveAt(0);
        foreach (var view in views)
        {
            game.Fabric.InitView((BaseView) view);
        }

       /* var navMeshSurfaces = stage.GetComponents<NavMeshSurface>();
        foreach (var surface in navMeshSurfaces)
        {
            surface.BuildNavMesh();
        }*/
       Bootstrap.Instance.ChangeGameState(GameStateID.Game);
      
    }
    
  
}
