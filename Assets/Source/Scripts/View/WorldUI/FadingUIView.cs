using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.View.WorldUI
{
    public class FadingUIView : MonoBehaviour
    {
        [SerializeField] private Graphic[] fadingObjects;

        public void AnimateFade(float endOpacity, float dur,GameObject link)
        {
            float a = fadingObjects[0].color.a;
            DOTween.Sequence()
                .SetLink(link)
                .Append(DOTween.To(() => a, x => a = x, endOpacity, dur)
                    .SetLink(link)
                    .OnUpdate(() =>
                    {
                        foreach (var fadingObject in fadingObjects)
                        {
                            var imageColor = fadingObject.color;
                            imageColor.a = a;
                            fadingObject.color = imageColor;
                        }
                    }))
                .Insert(0, transform.DOLocalMoveY(1, dur).SetRelative())
                .OnKill(() => { Destroy(gameObject); });
        }
    }
}