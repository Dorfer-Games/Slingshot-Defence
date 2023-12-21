using Kuhpik;
using Source.Scripts.Data.Enum;
using Source.Scripts.View.Cam;
using UnityEngine;

namespace Source.Scripts.System.Input
{
    public class SlingInputSystem : GameSystem
    {
        private Joystick joystick;
        private CameraSwitcherView cameraSwitcherView;
        private Animator playerAnimator;
        private float shotLen;

        public override void OnInit()
        {
            base.OnInit();
            joystick = game.Joystick;
           
            cameraSwitcherView = game.CameraSwitcherView;
            playerAnimator = game.PlayerView.StickmanAnimator;

            game.PlayerView.PlayerAnimatorView.ShotEvent += SendShotEvent;
            joystick.PointerUpEvent += Release;
            joystick.PointerDownEvent += Load;
            joystick.DragEvent += Drag;
            
            cameraSwitcherView.Switch(CameraPositionType.DEFAULT);
        }

        private void SendShotEvent()
        {
            pool.ShotEvent.Add(eventWorld.NewEntity()).ShotLen=shotLen;
        }

        private void Load()
        {
            ref var ammo = ref pool.Ammo.Get(game.PlayerEntity);
            if (ammo.Count == 0)
                return;
            
            var firstAmmo = ammo.Value[^ammo.Count];
            var elID = (int)pool.Element.Get(firstAmmo).Value;
            if (pool.Ult.Has(firstAmmo))
                elID += config.ElementsCount - 1;
            
            game.PlayerView.SlingBall.SetModel(elID);
            
            Time.timeScale = config.SlowTimeScale;
            cameraSwitcherView.Switch(CameraPositionType.AIMING);
            playerAnimator.Play("Pull");
        }

        private void Release()
        {
            Time.timeScale = 1;
            cameraSwitcherView.Switch(CameraPositionType.DEFAULT);
            var lineRenderer = game.PlayerView.LineRenderer;

            if (lineRenderer.positionCount>0)
            {
                shotLen = lineRenderer.GetPosition(lineRenderer.positionCount - 1).magnitude;
                playerAnimator.Play("Release");
            }
            else
            {
                pool.ShotCancelEvent.Add(eventWorld.NewEntity());
                playerAnimator.Play("Idle");
            }
        }

        private void Drag()
        {
            if (joystick.Direction.Equals(Vector2.zero))
                return;

            var normalized = joystick.Direction.normalized;
            pool.Dir.Get(game.PlayerEntity).Value =
                new Vector3(-normalized.x, 0, -normalized.y);
        }
    }
}