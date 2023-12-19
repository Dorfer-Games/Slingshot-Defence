using System;
using System.Collections;
using AYellowpaper.SerializedCollections;
using Nrjwolf.Tools.AttachAttributes;
using Source.Scripts.Data.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class CounterUIView : MonoBehaviour
    {
        [GetComponentInChildren()] [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField] private Image bg;

        [SerializeField] private Image icon;
        [SerializeField] private SerializedDictionary<ResType, Sprite> activeIcons;

        public void SetTextWithBgGrow(int value)
        {
            var fontSize = text.fontSize;
            var digits=0f;
            if (value==0)
                digits = 1;
            else
                digits = MathF.Floor(MathF.Log10(value) + 1);
            var offset = icon.rectTransform.rect.width / 2f;
            text.text = $"{value}";
            bg.rectTransform.sizeDelta = new Vector2(offset + digits * fontSize, bg.rectTransform.rect.height);
        }

        public void SetText(int value, int maxValue)
        {
            text.text = $"{value}/{maxValue}";
        }
        public void SetText(int value)
        {
            text.text = $"{value}";
        }
        
        public void SetText(string value)
        {
            text.text = value;
        }

        public void SetResImage(ResType costType)
        {
            icon.sprite = activeIcons[costType];
        }

        public void AnimateCount(float from, float to, float duration)
        {
            StartCoroutine(CountTo(from, to, duration));
        }

        private IEnumerator CountTo(float from, float to, float duration)
        {
            var rate = Mathf.Abs(to - from) / duration;
            while (Math.Abs(from - to) > 0.01)
            {
                from = Mathf.MoveTowards(from, to, rate * Time.deltaTime);
                SetText((int) from);
                yield return null;
            }

            SetText((int) to);
            if (to==0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}