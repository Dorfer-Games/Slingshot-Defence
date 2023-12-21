using System;
using Source.Scripts.Data;
using Source.Scripts.Data.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class UpCardView : MonoBehaviour
    {
        public Button Button;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image bg;
        [SerializeField] private Image icon;
        [SerializeField] private Image tomeBallicon;
        [SerializeField] private GameObject clickBlock;

        private UIConfig config;

        public void Init(UIConfig uiConfig)
        {
            config = uiConfig;
        }
        public void ToggleClickBlock(bool a)
        {
            clickBlock.gameObject.SetActive(a);
        }

        public void SetBall(ElementType elementType,int lvl)
        {
            tomeBallicon.gameObject.SetActive(false);
            nameText.text = config.ElementNames[elementType];
            levelText.text = $"LVL {lvl+1}";
            bg.sprite = config.ElementBG;
            icon.sprite = config.ElementIcons[elementType];
            icon.SetNativeSize();
        }
        
        public void SetUlt(ElementType elementType)
        {
            tomeBallicon.gameObject.SetActive(false);
            nameText.text = config.UltNames[elementType];
            levelText.text = $"EVOLUTION";
            bg.sprite = config.UltBG;
            icon.sprite = config.UltIcons[elementType];
            icon.SetNativeSize();
        }
        
        public void SetTome(TomeType tomeType,int lvl)
        {
            tomeBallicon.gameObject.SetActive(true);
            nameText.text = config.TomeNames[tomeType];
            levelText.text = $"LVL {lvl+1}";
            bg.sprite = config.TomeBG;
            icon.sprite = config.TomeIcons[tomeType];
            tomeBallicon.sprite = config.TomeBallIcons[tomeType];
            icon.SetNativeSize();
        }
    }
}