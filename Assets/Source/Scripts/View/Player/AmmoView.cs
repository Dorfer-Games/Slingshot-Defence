using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.View.Player
{
    public class AmmoView : MonoBehaviour
    {
        public Camera cam;
        public Transform FirstBallPos;
        public List<BaseView> AmmoBalls;
        
        private void Update()
        {
            cam.fieldOfView = 60;
        }
    }
}