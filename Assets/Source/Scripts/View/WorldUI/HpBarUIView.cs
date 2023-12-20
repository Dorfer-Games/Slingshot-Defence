using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.View.WorldUI
{
    public class HpBarUIView : MonoBehaviour
    {
        [SerializeField] private Image progressImage;
       
        [SerializeField] private FadingNumberUIView fadingNumberUIViewPrefab;
        [SerializeField] private FadingNumberUIView fadingGoldUIPrefab;

        public void SetValue(float p)
        {
            var transformLocalScale = progressImage.transform.localScale;
            transformLocalScale.x = p;
            progressImage.transform.localScale = transformLocalScale;
        }

        public void ToggleActive(bool isActive)
        {
            //gameObject.transform.parent.gameObject.SetActive(isActive);
            gameObject.SetActive(isActive);
        }

        public void AnimateDamageFadeNumber(int damage,GameObject link)
        {
            var fadingUIView = Instantiate(fadingNumberUIViewPrefab, transform);
            fadingUIView.Text.text = damage.ToString();
            fadingUIView.AnimateFade(0,0.4f,link);
        }
        
        public void AnimateGoldFadeNumber(int gold,GameObject link)
        {
            gameObject.transform.parent.gameObject.SetActive(true);
            var fadingUIView = Instantiate(fadingGoldUIPrefab, transform);
            fadingUIView.transform.SetParent(transform.parent);
            fadingUIView.Text.text = "+"+gold;
            fadingUIView.AnimateFade(1f,0.4f,link);
        }
    }
}