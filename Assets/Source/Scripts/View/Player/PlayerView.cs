using Source.Scripts.View.Player;
using TMPro;
using UnityEngine;

namespace Source.Scripts.View
{
    public class PlayerView : BaseView
    {
        public TriggerListenerView BodyTriggerListener;
        public Transform BallSpawnPos;
        public AmmoView AmmoView;
        public Canvas Canvas;
        public LineRenderer LineRenderer;
        public SkinnedMeshRenderer RopeMeshRenderer;
        public Animator StickmanAnimator;
        public ModelChangerView SlingBall;
        public PlayerAnimatorView PlayerAnimatorView;
        public ParticleSystem LvlUpVFX;


    }
}