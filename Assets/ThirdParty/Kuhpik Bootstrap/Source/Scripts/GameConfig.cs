using AYellowpaper.SerializedCollections;
using UnityEngine;
using NaughtyAttributes;
using Source.Scripts.Data.Enum;

namespace Kuhpik
{
    [CreateAssetMenu(menuName = "Config/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        public float PlayerSpeed;
        public float UnitRotSpeed;
        public float EnemySpeed;
        public int InsectSpawnChance;
        public float KnockbackForce;
        public float EnemyKnockbackForce;
        public float ZoneActivationTime;

        public SerializedDictionary<UpType,int[]> PlayerUps;
        public SerializedDictionary<UpType,int[]> PlayerUpsCosts;
    }
}