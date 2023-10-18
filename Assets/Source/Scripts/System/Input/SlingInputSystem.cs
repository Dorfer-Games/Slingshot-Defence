using Kuhpik;
using UnityEngine;

namespace Source.Scripts.System.Input
{
    public class SlingInputSystem : GameSystem
    {
        private Joystick joystick;
        private float offset;

        public override void OnInit()
        {
            base.OnInit();
            joystick = game.Joystick;
            offset = config.SlingInputOffset;

            joystick.PointerUpEvent += Release;
            joystick.PointerDownEvent += Load;
            joystick.DragEvent += Drag;
        }

        private void Load()
        {
            
        }

        private void Release()
        {
            if (joystick.Direction.sqrMagnitude>=offset*offset)
            {
                pool.ShotEvent.Add(eventWorld.NewEntity());
            }
            else
            {
                pool.ShotCancelEvent.Add(eventWorld.NewEntity());
            }
        }

        private void Drag()
        {
            base.OnUpdate();
            if (joystick.Direction.Equals(Vector2.zero))
                return;
            
            var normalized = joystick.Direction.normalized;
            pool.Dir.Get(game.PlayerEntity).Value =
                new Vector3(-normalized.x, 0, -normalized.y);
        }
    }
}