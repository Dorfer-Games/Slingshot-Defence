using Kuhpik;
using Source.Scripts.Data.Enum;
using UnityEngine;

namespace Source.Scripts.System.Input
{
    public class TrajectorySystem : GameSystem
    {
        private Joystick joystick;

        public override void OnInit()
        {
            base.OnInit();
            joystick = game.Joystick;
          
        }
        

        public override void OnUpdate()
        {
            base.OnUpdate();
        
            if (joystick.Direction.Equals(Vector2.zero))
            {
                game.PlayerView.LineRenderer.positionCount=0;
                return;
            }

            var normalized = joystick.Direction.normalized;
            pool.Dir.Get(game.PlayerEntity).Value =
                new Vector3(-normalized.x, 0, -normalized.y);

            var list = new Vector3[config.SlingPointsCount];
            var dir = game.PlayerView.BallSpawnPos.forward;
            var ballSpawnPos = game.PlayerView.BallSpawnPos.position;

            for (int i = 0; i < list.Length; i++)
            {
                list[i] =  ballSpawnPos+ dir*(i * 0.1f * config.BallSpeed);
            }

            var lineRenderer = game.PlayerView.LineRenderer;
            lineRenderer.positionCount = list.Length;
            lineRenderer.SetPositions(list);
        }
    }
}