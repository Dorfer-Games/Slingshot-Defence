using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
    public class ProgressBarView : MonoBehaviour
    {
        [SerializeField] private Image progressImage;
        [SerializeField] private TextMeshProUGUI counter;
        [SerializeField] private string postfix;

        public void SetProgress(int cur,int max)
        {
            float f = cur / (float) max;
            progressImage.fillAmount = f;
            counter.text = $"{cur}/{max}"+postfix;
        }
        
        public void SetMaxExp(bool showMaxText=true)
        {
            progressImage.fillAmount = 1f;
            if (showMaxText)
                counter.text = $"MAX";
        }
       
    }
}