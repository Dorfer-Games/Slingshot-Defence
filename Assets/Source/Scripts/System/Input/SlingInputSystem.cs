using Kuhpik;
using Source.Scripts.Data.Enum;
using Source.Scripts.View.Cam;
using UnityEngine;

namespace Source.Scripts.System.Input
{
    public class SlingInputSystem : GameSystem
    {
        private Joystick joystick;
        private float offset;
        private CameraSwitcherView cameraSwitcherView;
        private Animator playerAnimator;

        public override void OnInit()
        {
            base.OnInit();
            joystick = game.Joystick;
            offset = config.SlingInputOffset;
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
            pool.ShotEvent.Add(eventWorld.NewEntity());
        }

        private void Load()
        {
            ref var ammo = ref pool.Ammo.Get(game.PlayerEntity);
            if (ammo.Count == 0)
                return;
            
            var firstAmmo = ammo.Value[^ammo.Count];
            var elementType = pool.Element.Get(firstAmmo).Value;
            game.PlayerView.SlingBall.SetModel((int)elementType);
            
            Time.timeScale = config.SlowTimeScale;
            cameraSwitcherView.Switch(CameraPositionType.AIMING);
            playerAnimator.Play("Pull");
        }

        private void Release()
        {
            Time.timeScale = 1;
            cameraSwitcherView.Switch(CameraPositionType.DEFAULT);
           
            
            if (joystick.Direction.sqrMagnitude >= offset * offset)
            {
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