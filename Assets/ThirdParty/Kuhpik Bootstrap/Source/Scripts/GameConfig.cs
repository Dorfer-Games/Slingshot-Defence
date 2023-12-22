using AYellowpaper.SerializedCollections;
using UnityEngine;
using NaughtyAttributes;
using Source.Scripts.Component.Battle;
using Source.Scripts.Component.Battle.Ball;
using Source.Scripts.Component.Battle.Tome;
using Source.Scripts.Component.Tome;
using Source.Scripts.Data;
using Source.Scripts.Data.Balls;
using Source.Scripts.Data.Enum;
using Source.Scripts.Data.Ults;
using Source.Scripts.View;

namespace Kuhpik
{
    [CreateAssetMenu(menuName = "Config/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        public float StageMaxZ=30f;
        public int MaxLevels;
        public int ElementsCount = 6;
        public int MaxSimpleBallLevel = 4;
        public float ScaleFactor;
        public int SlingPointsCount;
        //public float SlingInputOffset;
        public float UnitRotSpeed;
        public float BallSpeed;
        public LayerMask EnemyLayerMask;


        [Header("Settable")] 
        public float SlingPullPercentToCrit;
        public float MultishotAngle;
        public float KnockbackTime;
        public float BallBaseRadius;
        public float BallBaseKnockback;
        public float SlowTimeScale;
        public float HpToDmgK;


        [BoxGroup("Ups")] public int[] ExpProgression;
        [BoxGroup("Ups")] public int SameTomeInRollChancePercent;
        [BoxGroup("Ups")] public int SameBallInRollChancePercent;


        [BoxGroup("Tomes")] public Through[] ThroughTome;
        [BoxGroup("Tomes")] public Radius[] RadiusTome;
        [BoxGroup("Tomes")] public Ricochet[] RicochetTome;
        [BoxGroup("Tomes")] public Knockback[] KnockbackTome;
        [BoxGroup("Tomes")] public Mult[] MultTome;

        [BoxGroup("Balls")] public FireData[] FireBall;
        [BoxGroup("Balls")] public DarkData[] DarkBall;
        [BoxGroup("Balls")] public LightningData[] LightningBall;
        [BoxGroup("Balls")] public BoulderData[] BoulderBall;
        [BoxGroup("Balls")] public SlimeData[] SlimeBall;
        [BoxGroup("Balls")] public SerializedDictionary<ElementType,int[]> AddDamagePercentProg;
        
        [BoxGroup("Ults")] public MeteorData MeteorUlt;
        [BoxGroup("Ults")] public SuperDarkData SuperDarkUlt;
        [BoxGroup("Ults")] public ThunderBallData ThunderBallUlt;
        [BoxGroup("Ults")] public SuperBoulderData SuperBoulderUlt;
        [BoxGroup("Ults")] public SuperSlimeData SuperSlimeUlt;
       


        [BoxGroup("Configs")] public SerializedDictionary<EnemyType, EnemyConfig> EnemyConfigs;
        [BoxGroup("Configs")]  public SerializedDictionary<SlingType, SlingConfig> SlingConfigs;
        [BoxGroup("Configs")]  public UIConfig UIConfig;
        [BoxGroup("Configs")]  public WorldMenuConfig WorldMenuDataConfig;


        [Header("Prefabs")]
        [BoxGroup("Prefabs")] public BaseView BallPrefab;
        [BoxGroup("Prefabs")] public SerializedDictionary<ElementType,BaseView> ZonesPrefabs;
        [BoxGroup("Prefabs")] public SerializedDictionary<ElementType,GameObject> ZoneSpreadPrefabs;
        [BoxGroup("Prefabs")] public GameObject BaseHitVFXPrefab;
    }
}