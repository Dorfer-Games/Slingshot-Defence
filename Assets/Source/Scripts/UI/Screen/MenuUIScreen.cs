using AYellowpaper.SerializedCollections;
using Kuhpik;
using Source.Scripts.Data.Enum;
using Source.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MenuUIScreen : UIScreen
{
    [SerializeField] private Button playBtn;
    [SerializeField] private TextMeshProUGUI stagesText;
    [SerializeField] private TextMeshProUGUI worldText;
    [SerializeField] private CounterUIView goldCounter;
    public MenuItemView LibItem;
    public MenuItemView WorldItem;
    public MenuItemView UpsItem;
    public MenuItemView[] MenuItems;
    public SerializedDictionary<UpType, SlingUpView> SlingUps;

    public override void Subscribe()
    {
        base.Subscribe();
        playBtn.onClick.AddListener(() =>
        {
            Bootstrap.Instance.SaveGame();
            Bootstrap.Instance.ChangeGameState(GameStateID.Loading);
        });

        foreach (var menuItem in MenuItems)
        {
            menuItem.Button.onClick.AddListener(() =>
            {
                //deselect all
                foreach (var item in MenuItems)
                    item.ToggleSelected(false);
                
                //select
                menuItem.ToggleSelected(true);
                menuItem.transform.SetAsLastSibling();
            });

            var goldCounterActive = !menuItem.Equals(LibItem);
            menuItem.Button.onClick.AddListener(() =>
            {
                goldCounter.gameObject.SetActive(goldCounterActive);
            });
        }
    }

    public void SetGold(int value)
    {
        goldCounter.SetText(value);
    }

    public void SelectItem(MenuItemView targetItem)
    {
        foreach (var item in MenuItems)
            item.ToggleSelected(false);
                
        //select
        targetItem.ToggleSelected(true);
        targetItem.transform.SetAsLastSibling();
    }

    public void SetStagesText(int completeCount,int count)
    {
        stagesText.text = $"Stages: {completeCount}-{count}";
    }
    public void SetWorldText(string name)
    {
        worldText.text = name;
    }
}
