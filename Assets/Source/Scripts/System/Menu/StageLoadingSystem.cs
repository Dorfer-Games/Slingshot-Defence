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

public class StageLoadingSystem : GameSystem
{
    public override void OnInit()
    {
        base.OnInit();

        var level = Mathf.Clamp(save.StageToLoad, 0, config.MaxLevels - 1);
        var prefab = Resources.Load<StageView>(string.Format("Stages/Stage {0}", level + 1));
        var go=Instantiate(prefab);

        if (save.SkipMenu)
        {
            save.SkipMenu = false;
            Bootstrap.Instance.SaveGame();
            StartCoroutine(WaitForStageInit(go.gameObject));
        }
    }

    private IEnumerator WaitForStageInit(GameObject go)
    {
        yield return new WaitUntil(() => go.activeSelf);
        Bootstrap.Instance.ChangeGameState(GameStateID.Loading);
    }


}
