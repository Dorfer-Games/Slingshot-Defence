using DG.Tweening;
using Kuhpik;
using Source.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIScreen : UIScreen
{
    [SerializeField] private ProgressBarView expBar;
    [SerializeField] private ProgressBarView hpBar;
    [SerializeField] private CounterUIView goldCounter;
    [SerializeField] private GameObject reloadGO;

    public void ToggleReload(bool a)
    {
        reloadGO.gameObject.SetActive(a);
        if (a)
        {
            reloadGO.transform.DORotate(new Vector3(0,0,-360),1f,RotateMode.FastBeyond360);
        }
    }

    public void SetHp(int cur, int toGet)
    {
        hpBar.SetProgress(cur, toGet);
    }

    public void SetExp(int cur, int toGet)
    {
        expBar.SetProgress(cur, toGet);
    }

    public void SetGold(int count)
    {
        goldCounter.SetText(count);
    }

    public void SetMaxExp()
    {
        expBar.SetMaxExp();
    }
}