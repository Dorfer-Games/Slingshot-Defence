using System;
using UnityEngine;

namespace Source.Scripts.View
{
    public class PlayerAnimatorView : MonoBehaviour
    {
        public event Action ShotEvent;
        public void OnShot()
        {
            ShotEvent?.Invoke();
        }
    }
}