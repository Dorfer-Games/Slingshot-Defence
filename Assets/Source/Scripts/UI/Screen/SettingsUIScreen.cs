using System;
using Kuhpik;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI.Screen
{
    public class SettingsUIScreen : UIScreen
    {
        [SerializeField] private GameObject settings;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button hapticButton;
        [SerializeField] private Sprite hapticOn;
        [SerializeField] private Sprite hapticOff;

        public event Action OnSettingsButtonClick;
        public event Action OnHapticButtonClick;

        public override void Subscribe()
        {
            settingsButton.onClick.AddListener(SendSettingsButtonClickEvent);
            hapticButton.onClick.AddListener(SendHapticButtonClickEvent);
        }

        private void SendSettingsButtonClickEvent()
        {
            OnSettingsButtonClick?.Invoke();
        }

        private void SendHapticButtonClickEvent()
        {
            OnHapticButtonClick?.Invoke();
        }

        public void ToggleSettings()
        {
            settings.SetActive(!settings.activeSelf);
        }

        public void ToggleHaptic(bool isEnabled)
        {
            var sprite = isEnabled ? hapticOn : hapticOff;
            hapticButton.image.sprite = sprite;
        }
    }
}