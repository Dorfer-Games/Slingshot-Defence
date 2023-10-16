using UnityEngine;

namespace Source.Scripts.View
{
    public class BaseView : MonoBehaviour
    {
        [HideInInspector]
        public int Entity;

        public  void Die()
        {
            Destroy(gameObject);
        }
        
        public void ToggleActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}