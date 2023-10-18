using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.Event;
using UnityEngine;

namespace Source.Scripts.System.Sling.Shot
{
    public class ShotSystem : GameSystem
    {
        private EcsFilter filter;
        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<ShotEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filter)
            {
                var ball = Instantiate(config.BallPrefab);
                ball.transform.position = game.PlayerView.BallSpawnPos.transform.position;
                var ent = game.Fabric.InitView(ball);
                pool.Dir.Get(ent).Value = game.PlayerView.transform.forward;
                pool.Speed.Add(ent).Value = config.BallSpeed;
            }
        }
    }
}