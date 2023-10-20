using AYellowpaper.SerializedCollections;
using Source.Scripts.Data.Enum;
using UnityEngine;

namespace Source.Scripts.View.Player
{
    public class UpsView : MonoBehaviour
    {
        public SerializedDictionary<UpType, int> Ups;
    }
}