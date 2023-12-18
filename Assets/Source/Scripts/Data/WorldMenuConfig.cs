using AYellowpaper.SerializedCollections;
using Source.Scripts.Data.Enum;
using Source.Scripts.View;
using UnityEngine;

namespace Source.Scripts.Data
{
    [CreateAssetMenu(menuName = "Config/WorldMenuConfig")]
    public class WorldMenuConfig : ScriptableObject
    {
        public WorldMenuData[] WorldMenuDatas;

    }
}