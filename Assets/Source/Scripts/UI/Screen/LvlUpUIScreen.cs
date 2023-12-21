using Kuhpik;
using Source.Scripts.UI;
using UnityEngine;


public class LvlUpUIScreen : UIScreen
{
    public UpCardView[] UpCards;
    [SerializeField] private GameObject tutorPointer;
    
    public void TogglePointer(bool a)
    {
        tutorPointer.gameObject.SetActive(a);
    }
}
