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
                ref var ammo = ref pool.Ammo.Get(game.PlayerEntity);
                if (ammo.Count == 0)
                    return;

                var ent = SpawnBall();

                //ammo
                var firstAmmo = ammo.Value[^ammo.Count];
                pool.View.Get(firstAmmo).Value.ToggleActive(false);
                ammo.Count--;
                
                //ball perks
                var elementType = pool.Element.Get(firstAmmo).Value;
                pool.ModelChangerComponent.Get(ent).Value.SetModel((int)elementType);

                pool.Damage.Add(ent).Value = 10;//!
            }
        }

        private int SpawnBall()
        {
            var ball = Instantiate(config.BallPrefab);
            ball.transform.position = game.PlayerView.BallSpawnPos.transform.position;
            var ent = game.Fabric.InitView(ball);
            pool.Dir.Get(ent).Value = game.PlayerView.transform.forward;
            pool.Speed.Add(ent).Value = config.BallSpeed;
            pool.SpawnBallEvent.Add(eventWorld.NewEntity()).Value = ent;
            pool.Radius.Add(ent).Value = config.BallBaseRadius;
            pool.Knockback.Add(ent).Value = config.BallBaseKnockback;

            return ent;
        }
    }
}