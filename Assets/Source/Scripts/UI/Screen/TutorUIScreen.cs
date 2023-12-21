using Kuhpik;
using UnityEngine;

namespace Source.Scripts.UI.Screen
{
    public class TutorUIScreen : UIScreen
    {
        [SerializeField] private GameObject controlsScreen;
        [SerializeField] private GameObject ammoScreen;
        
        public void ToggleControlsScreen(bool a)
        {
            controlsScreen.gameObject.SetActive(a);
        }  

        public void ToggleAmmoScreen(bool a)
        {
            ammoScreen.gameObject.SetActive(a);
        }
    }
}