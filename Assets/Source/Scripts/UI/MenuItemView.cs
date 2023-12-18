using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class MenuItemView : MonoBehaviour
    {
        [SerializeField] private GameObject selectedImage;
        public Button Button;
        public GameObject OpenGo;

        public void ToggleSelected(bool a)
        {
            selectedImage.gameObject.SetActive(a);
            OpenGo.gameObject.SetActive(a);
        }
    }
}