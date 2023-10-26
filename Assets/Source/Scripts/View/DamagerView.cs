using UnityEngine;

namespace Source.Scripts.View
{
    public class DamagerView : MonoBehaviour
    {
        public TriggerListenerComponent AttackTriggerListener;
         
        
        private void OnDrawGizmos()
        {
            Gizmos.color=Color.black;
            Gizmos.DrawSphere(transform.position,3);
        }
    }
}