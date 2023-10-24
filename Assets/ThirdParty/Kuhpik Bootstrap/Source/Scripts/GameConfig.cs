using AYellowpaper.SerializedCollections;
using UnityEngine;
using NaughtyAttributes;
using Source.Scripts.Data;
using Source.Scripts.Data.Enum;
using Source.Scripts.View;

namespace Kuhpik
{
    [CreateAssetMenu(menuName = "Config/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        public int SlingPointsCount;
        public float SlingInputOffset;
        public float UnitRotSpeed;
      
        public float BallSpeed;
        public float ReloadTime;
        public float SlowTimeScale;
        public float KnockbackForce;
        
        public int[] ExpProgression;
        
        public SerializedDictionary<EnemyType,EnemyConfig> EnemyUps;

        public SerializedDictionary<UpType,int[]> PlayerUps;
        public SerializedDictionary<UpType,int[]> PlayerUpsCosts;

        [Header("Prefabs")]
        public BaseView BallPrefab;
    }
}