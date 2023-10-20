using AYellowpaper.SerializedCollections;
using UnityEngine;
using NaughtyAttributes;
using Source.Scripts.Data.Enum;
using Source.Scripts.View;

namespace Kuhpik
{
    [CreateAssetMenu(menuName = "Config/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        public float SlingInputOffset;
        public float UnitRotSpeed;
        public float EnemySpeed;
        public float BallSpeed;
        public float ReloadTime;
        public float SlowTimeScale;
        public int InsectSpawnChance;
        public float KnockbackForce;
        public float EnemyKnockbackForce;
        public float ZoneActivationTime;

        public SerializedDictionary<UpType,int[]> PlayerUps;
        public SerializedDictionary<UpType,int[]> PlayerUpsCosts;

        [Header("Prefabs")]
        public BaseView BallPrefab;
    }
}