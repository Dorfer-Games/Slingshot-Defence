using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Event;
using Source.Scripts.Component.Movement;
using Source.Scripts.Component.ViewComponent;
using UnityEngine;

namespace Source.Scripts.System.Move
{
    public class RemoveDeadFromWaveSystem : GameSystem
    {
        private EcsFilter filter;
        
        public override void OnInit()
        {
            base.OnInit();

            filter = world.Filter<Enemy>().Inc<DeadTag>().Exc<DeathAnimTick>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var ent in filter)
            {
                ref var stage = ref pool.Stage.Get(game.StageEntity);
                stage.AliveEnemies.Remove(ent);
            }
        }

    }
}