using Kuhpik;
using Source.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIScreen : UIScreen
{
    [SerializeField] private Image expFillBar;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private CounterUIView goldCounter;


    public void SetExp(int cur,int toGet)
    {
        float f = cur / (float) toGet;
        expFillBar.fillAmount = f;
        expText.text = $"{cur}/{toGet} XP";
    }

    public void SetGold(int count)
    {
        goldCounter.SetText(count);
    }

    public void SetMaxExp()
    {
        expFillBar.fillAmount = 1f;
        expText.text = $"MAX";
    }
}
