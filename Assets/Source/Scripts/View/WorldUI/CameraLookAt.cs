using UnityEngine;

namespace Source.Scripts.View.WorldUI
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