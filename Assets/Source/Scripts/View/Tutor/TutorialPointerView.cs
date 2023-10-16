using NaughtyAttributes;
using UnityEngine;

namespace Source.Scripts.View
{
    public class TutorialPointerView : MonoBehaviour
    {
        [SerializeField] private bool _move;
        [SerializeField] private Vector3 offset;


        [SerializeField, ShowIf(nameof(_move))]
        private float _speed;

        [SerializeField, ShowIf(nameof(_move)), MinMaxSlider(0, 10)]
        private Vector2 _sizeRange;

        private Vector3 _minSize;
        private Vector3 _maxSize;

        private float _time;
        private Transform target;
        private float bound;
        private Transform player;


        public void SetTarget(Transform target)
        {
            this.target = target;
            bound = GetBound(target.gameObject);
        }

        private void Awake()
        {
            player = transform.parent;
            transform.SetParent(null);
            _minSize = Vector3.one * _sizeRange.x;
            _maxSize = Vector3.one * _sizeRange.y;
        }

        private void Update()
        {
            if (target != null)
                Direct();
            if (!_move) return;
            _time += Time.deltaTime * _speed;

            var clampTime = Mathf.Cos(_time);
            clampTime = Remap(clampTime, -1, 1, 0, 1);

            transform.localScale = Vector3.Lerp(_minSize, _maxSize, clampTime);
        }

        private float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        private void Direct()
        {
            var targetPosition = target.position;
            var playerPosition = player.position;

            var distance = Vector3.Distance(targetPosition, playerPosition);
            var toTargetDirection = targetPosition - playerPosition;

            var farPosition = playerPosition + Vector3.up + (toTargetDirection.normalized / 2);
            var nerPosition = targetPosition + Vector3.up + Vector3.up * bound;

            var lerpT = Mathf.Clamp(distance, 1.5f, 2.5f);
            lerpT = Remap(lerpT, 1.5f, 2.5f, 0, 1);
            transform.position = Vector3.Lerp(nerPosition, farPosition, lerpT)+offset;

            transform.LookAt(targetPosition);
        }

        private float GetBound(GameObject go)
        {
            var renderer = go.GetComponent<Renderer>();
            if (renderer == null) renderer = go.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                return renderer.bounds.size.y;
            }
            else
            {
                return 1;
            }
        }
    }
}