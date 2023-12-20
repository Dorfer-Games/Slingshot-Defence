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

            int pointsCount = (int)(config.SlingPointsCount * joystick.Direction.magnitude);
            var list = new Vector3[pointsCount];
            var dir = game.PlayerView.BallSpawnPos.forward;
            var ballSpawnPos = game.PlayerView.BallSpawnPos.position;

            float k = 0.01f /*/ config.SlingPointsCount*/;
            for (int i = 0; i < pointsCount; i++)
            {
                list[i] =  ballSpawnPos+ dir*(i * k * config.BallSpeed);
            }

            var lineRenderer = game.PlayerView.LineRenderer;
            lineRenderer.positionCount = list.Length;
            lineRenderer.SetPositions(list);
        }
    }
}