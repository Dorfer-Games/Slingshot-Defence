using Source.Scripts.View.Player;
using TMPro;
using UnityEngine;

namespace Source.Scripts.View
{
    public class PlayerView : BaseView
    {
        public TriggerListenerComponent BodyTriggerListener;
        public Transform BallSpawnPos;
        public AmmoView AmmoView;
        public Canvas Canvas;
        public LineRenderer LineRenderer;


    }
}