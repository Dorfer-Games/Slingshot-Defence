using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class SlingUpView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI costText;
        public Button Button;

        public void SetCost(int value,int playerGold)
        {
            costText.text = $"{value}";
            Button.interactable = playerGold >= value;
        }

        public void SetMax()
        {
            costText.text = "MAX";
            Button.interactable = false;
        }
    }
}