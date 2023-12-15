using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle;
using Source.Scripts.Component.Event;
using Source.Scripts.Component.Movement;
using Source.Scripts.Component.ViewComponent;
using UnityEngine;

namespace Source.Scripts.System.Move
{
    public class NextStageSwitchSystem : GameSystem
    {
        private EcsFilter filter;
        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<NextStageEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            foreach (var ent in filter)
            {
                save.StageToLoad++;
                save.SkipMenu = true;
                Bootstrap.Instance.SaveGame();
                Bootstrap.Instance.GameRestart(0);
            }
        
        }
    }
}