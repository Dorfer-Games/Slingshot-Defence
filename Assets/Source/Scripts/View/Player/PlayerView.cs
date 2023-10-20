using Source.Scripts.View.Player;
using TMPro;
using UnityEngine;

namespace Source.Scripts.View
{
    public class PlayerView : BaseView
    {
        public GameObject[] Models;
        public TriggerListenerComponent Body;
        public Transform BallSpawnPos;
        public AmmoView AmmoView;
        public Canvas Canvas;

       
    }
}