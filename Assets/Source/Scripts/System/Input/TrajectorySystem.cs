using Kuhpik;
using Source.Scripts.Data.Enum;
using UnityEngine;

namespace Source.Scripts.System.Input
{
    public class TrajectorySystem : GameSystem
    {
        private Joystick joystick;
        private LineRenderer lineRenderer=null;
        private SkinnedMeshRenderer playerMesh;

        public override void OnInit()
        {
            base.OnInit();
            joystick = game.Joystick;
            joystick.PointerUpEvent += ()=>
            {
                ToggleRed(false);
            };
            lineRenderer=game.PlayerView.LineRenderer;
            playerMesh = game.PlayerView.GetComponentInChildren<SkinnedMeshRenderer>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        
            if (joystick.Direction.Equals(Vector2.zero))
            {
                lineRenderer.positionCount=0;
                return;
            }

            int pointsCount = (int)(config.SlingPointsCount * joystick.Direction.magnitude);
            var slingPullPercent = 100* pointsCount / (float) config.SlingPointsCount;
            
            ToggleRed(slingPullPercent >= config.SlingPullPercentToCrit);
            SetPoints(pointsCount);
        }

        private void SetPoints(int pointsCount)
        {
            var list = new Vector3[pointsCount];
            var dir = game.PlayerView.BallSpawnPos.forward;
            var ballSpawnPos = game.PlayerView.BallSpawnPos.position;

            float k = 0.01f /*/ config.SlingPointsCount*/;
            for (int i = 0; i < pointsCount; i++)
            {
                list[i] =  ballSpawnPos+ dir*(i * k * config.BallSpeed);
            }
            
            lineRenderer.positionCount = list.Length;
            lineRenderer.SetPositions(list);
        }

        private void ToggleRed(bool a)
        {
           
            if (a)
            {
                playerMesh.material.SetColor("_MainColor",Color.red);
                lineRenderer.endColor=Color.red;
                lineRenderer.startColor=Color.red;
            }
            else
            {
                playerMesh.material.SetColor("_MainColor",Color.white);
                lineRenderer.endColor=Color.red;
                lineRenderer.startColor=Color.white;
            }
         
        }
    }
}