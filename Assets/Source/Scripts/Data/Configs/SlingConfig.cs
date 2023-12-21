using AYellowpaper.SerializedCollections;
using Source.Scripts.Data.Enum;
using Source.Scripts.View;
using UnityEngine;

namespace Source.Scripts.Data
{
    [CreateAssetMenu(menuName = "Config/SlingConfig")]
    public class SlingConfig : ScriptableObject
    {
        public SerializedDictionary<UpType, float[]> Ups;
        public SerializedDictionary<UpType, int[]> UpCosts;
     
    }
}