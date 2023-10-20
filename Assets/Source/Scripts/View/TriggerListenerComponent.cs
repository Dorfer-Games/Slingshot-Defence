using System;
using UnityEngine;

namespace Source.Scripts.View
{
    public class TriggerListenerComponent : MonoBehaviour
    {
        public event Action<Transform,Transform> OnTriggerEnterEvent;
        public event Action<Transform,Transform> OnTriggerExitEvent;


        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(transform,other.transform);
        }
        
        
        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitEvent?.Invoke(transform,other.transform);
        }
    }
}