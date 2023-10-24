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
        
        public float BallBaseKnockback;
        public float BallBaseRadius;

        //settable
        public float ReloadTime;
        public float SlowTimeScale;
        public float HpToDmgK;


        public int[] ExpProgression;

        public SerializedDictionary<EnemyType,EnemyConfig> EnemyConfigs;

        public SerializedDictionary<SlingType,SlingConfig> SlingConfigs;


        [Header("Prefabs")]
        public BaseView BallPrefab;
    }
}