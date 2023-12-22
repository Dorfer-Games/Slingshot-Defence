using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class SlingUpView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI curValueText;
        [SerializeField] private TextMeshProUGUI nextValueText;
        public Button Button;

        public void SetCost(int value,int playerGold)
        {
            costText.text = $"{value}";
            ToggleInter(playerGold >= value);
        }
        public void SetValues(int cur,int next)
        {
            curValueText.text = $"{cur}";
            nextValueText.text = $"{next}";
        }
        public void SetValues(float cur,float next)
        {
            curValueText.text = $"{Math.Round(cur,2)}";
            nextValueText.text = $"{Math.Round(next,2)}";
        }

        private void ToggleInter(bool active)
        {
            float a = active?1f:0.5f;
            Button.interactable = active;
            SetA(a, costText);
            SetA(a, curValueText);
            SetA(a, nextValueText);
        }

        private void SetA(float a,TextMeshProUGUI tmp)
        {
            var color = tmp.color;
            color.a = a;
            tmp.color = color;
        }

        public void SetMax()
        {
            costText.text = "MAX";
            curValueText.text = "MAX";
            nextValueText.text = "MAX";
            ToggleInter(false);
        }
    }
}