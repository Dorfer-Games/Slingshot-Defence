using AYellowpaper.SerializedCollections;
using Source.Scripts.View;
using UnityEngine;

namespace Source.Scripts.Data
{
    [CreateAssetMenu(menuName = "Config/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        public Stats[] LevelStats;
        public BaseView Prefab;
    }
}