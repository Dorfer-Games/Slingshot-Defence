using NaughtyAttributes;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class TutorialUIPointerView : MonoBehaviour
    {
        [SerializeField] private bool _move;

        [SerializeField, ShowIf(nameof(_move))]
        private float _speed;

        [SerializeField, ShowIf(nameof(_move)), MinMaxSlider(0, 10)]
        private Vector2 _sizeRange;

        private Vector3 _minSize;
        private Vector3 _maxSize;

        private float _time;
        
    
        private void Awake()
        {
            _minSize = Vector3.one * _sizeRange.x;
            _maxSize = Vector3.one * _sizeRange.y;
        }

        private void Update()
        {
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

     
    }
}