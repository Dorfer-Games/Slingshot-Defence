using AYellowpaper.SerializedCollections;
using UnityEngine;
using NaughtyAttributes;
using Source.Scripts.Component.Battle;
using Source.Scripts.Component.Battle.Ball;
using Source.Scripts.Component.Battle.Tome;
using Source.Scripts.Component.Tome;
using Source.Scripts.Data;
using Source.Scripts.Data.Enum;
using Source.Scripts.View;

namespace Kuhpik
{
    [CreateAssetMenu(menuName = "Config/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        public float ScaleFactor;
        public int SlingPointsCount;
        public float SlingInputOffset;
        public float UnitRotSpeed;
        public float BallSpeed;


        [Header("Settable")] 
        public float SlimeZoneDuration;
        public float MultishotAngle;
        public float KnockbackTime;
        public float BallBaseRadius;
        public float BallBaseKnockback;
        public float ReloadTime;
        public float SlowTimeScale;
        public float HpToDmgK;


        public int[] ExpProgression;


        [BoxGroup("Tomes")] public Through[] ThroughTome;
        [BoxGroup("Tomes")] public Radius[] RadiusTome;
        [BoxGroup("Tomes")] public Ricochet[] RicochetTome;
        [BoxGroup("Tomes")] public Knockback[] KnockbackTome;
        [BoxGroup("Tomes")] public Mult[] MultTome;

        [BoxGroup("Balls")] public Fire[] FireBall;
        [BoxGroup("Balls")] public Dark[] DarkBall;
        [BoxGroup("Balls")] public Lightning[] LightningBall;
        [BoxGroup("Balls")] public Boulder[] BoulderBall;
        [BoxGroup("Balls")] public Slime[] SlimeBall;


        [BoxGroup("Configs")] public SerializedDictionary<EnemyType, EnemyConfig> EnemyConfigs;
        [BoxGroup("Configs")]  public SerializedDictionary<SlingType, SlingConfig> SlingConfigs;


        [Header("Prefabs")]
        [BoxGroup("Prefabs")] public BaseView BallPrefab;
        [BoxGroup("Prefabs")] public BaseView SlimeZonePrefab;
    }
}