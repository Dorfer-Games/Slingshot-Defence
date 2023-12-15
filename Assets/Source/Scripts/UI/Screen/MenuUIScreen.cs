using Kuhpik;

using UnityEngine;
using UnityEngine.UI;


public class MenuUIScreen : UIScreen
{
    [SerializeField] private Button button;

    public override void Subscribe()
    {
        base.Subscribe();
        button.onClick.AddListener(() =>
        {
            Bootstrap.Instance.ChangeGameState(GameStateID.Loading);
        });
    }
}
