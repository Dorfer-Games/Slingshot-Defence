using Kuhpik;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Source.Scripts.UI.Screen
{
    public class TutorUIScreen : UIScreen
    {
        [SerializeField] private GameObject controlsScreen;
        [SerializeField] private GameObject ammoScreen;
        [SerializeField] private GameObject lockScreen;
        
        public void ToggleControlsScreen(bool a)
        {
            controlsScreen.gameObject.SetActive(a);
        }  

        public void ToggleAmmoScreen(bool a)
        {
            ammoScreen.gameObject.SetActive(a);
        }

        public void ToggleLockInput(bool a)
        {
            lockScreen.gameObject.SetActive(a);
        }
    }
}