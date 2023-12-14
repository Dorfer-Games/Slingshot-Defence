using AYellowpaper.SerializedCollections;
using Source.Scripts.Data.Enum;
using Source.Scripts.View;
using UnityEngine;

namespace Source.Scripts.Data
{
    [CreateAssetMenu(menuName = "Config/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        public SerializedDictionary<ElementType, Sprite> ElementIcons;
        public SerializedDictionary<ElementType, Sprite> UltIcons;
        public SerializedDictionary<TomeType, Sprite> TomeIcons;
        public SerializedDictionary<TomeType, Sprite> TomeBallIcons;
        
        public SerializedDictionary<ElementType, string> ElementNames;
        public SerializedDictionary<ElementType, string> UltNames;
        public SerializedDictionary<TomeType, string> TomeNames;

        public Sprite ElementBG;
        public Sprite UltBG;
        public Sprite TomeBG;
    }
}