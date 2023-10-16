using System;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class CameraLookAt : MonoBehaviour
    {
        private Camera cam;
        private void Awake()
        {
            cam = Camera.main;
        }

        void Update()
        {
            transform.rotation = cam.transform.rotation;
        }
    }
}