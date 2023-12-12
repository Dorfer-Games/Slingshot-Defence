using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle;
using Source.Scripts.Component.Battle.Ball;
using Source.Scripts.Component.Battle.Tome;
using Source.Scripts.Component.Event;
using Source.Scripts.Component.Movement;
using Source.Scripts.Component.Tome;
using Source.Scripts.Component.ViewComponent;
using UnityEngine;

namespace Source.Scripts.System.Util
{
    public class Pools
    {
        public readonly EcsPool<BaseViewComponent> View;
        public readonly EcsPool<Direction> Dir;
        public readonly EcsPool<Speed> Speed;
        public readonly EcsPool<Hp> Hp;
        public readonly EcsPool<AnimationComponent> Anim;
        public readonly EcsPool<Inventory> Inventory;
        public readonly EcsPool<CantMoveTag> CantMove;
        public readonly EcsPool<RigidbodyComponent> Rb;
        public readonly EcsPool<DeadTag> Dead;
        public readonly EcsPool<SlingUps> SlingUps;
        public readonly EcsPool<Level> Level;
        public readonly EcsPool<Element> Element;
        public readonly EcsPool<ModelChangerComponent> ModelChangerComponent;
        public readonly EcsPool<Radius> Radius;
        public readonly EcsPool<Enemy> Enemy;
        public readonly EcsPool<Through> Through;
        public readonly EcsPool<Ricochet> Ricochet;
        public readonly EcsPool<Obstacle> Obstacle;
        public readonly EcsPool<Ammo> Ammo;
        public readonly EcsPool<MaxSpeed> MaxSpeed;
        public readonly EcsPool<ReloadTick> ReloadTick;
        public readonly EcsPool<EnemySpawnTick> EnemySpawnTick;
        public readonly EcsPool<WaveDelayTick> WaveDelayTick;
        public readonly EcsPool<Stage> Stage;
        public readonly EcsPool<NavMeshAgentComponent> NavMeshAgentComponent;
        public readonly EcsPool<Exp> Exp;
        public readonly EcsPool<Damage> Damage;
        public readonly EcsPool<Knockback> Knockback;
        public readonly EcsPool<Tomes> Tomes;
        public readonly EcsPool<Mult> Mult;
        public readonly EcsPool<Elements> Elements;
        public readonly EcsPool<Fire> Fire;
        public readonly EcsPool<Explosive> Explosive;
        public readonly EcsPool<Lightning> Lightning;
        public readonly EcsPool<Slime> Slime;
        public readonly EcsPool<PrevHitTargets> PrevHitTargets;
        public readonly EcsPool<KnockedTick> KnockedTick;
        public readonly EcsPool<BurnTick> BurnTick;
        public readonly EcsPool<HpViewComponent> HpViewComponent;
        public readonly EcsPool<HitVFXProviderComponent> HitVFXProviderComponent;
        public readonly EcsPool<Lifetime> Lifetime;
        public readonly EcsPool<ZoneTriggers> ZoneTriggers;
        public readonly EcsPool<Zone> Zone;
        public readonly EcsPool<ZoneSpread> ZoneSpread;
        public readonly EcsPool<Ult> Ult;
        public readonly EcsPool<Weakness> Weakness;
        public readonly EcsPool<KnockbackWave> KnockbackWave;
        public readonly EcsPool<DeathAnimTick> DeathAnimTick;



        //events
        public readonly EcsPool<HitEvent> HitEvent;
        public readonly EcsPool<ShotCancelEvent> ShotCancelEvent;
        public readonly EcsPool<ShotEvent> ShotEvent;
        public readonly EcsPool<SpawnBallEvent> SpawnBallEvent;
        public readonly EcsPool<DamageEvent> DamageEvent;
        public readonly EcsPool<HitPlayerEvent> HitPlayerEvent;
        public readonly EcsPool<SpawnZoneEvent> SpawnZoneEvent;
        public readonly EcsPool<SetOnFireEvent> SetOnFireEvent;
        public readonly EcsPool<ZoneExitEvent> ZoneExitEvent;
        public readonly EcsPool<ZoneEnterEvent> ZoneEnterEvent;
        public readonly EcsPool<VFXEvent> VFXEvent;
        public readonly EcsPool<KnockbackEvent> KnockbackEvent;

        public Pools(EcsWorld world, EcsWorld eventWorld)
        {
            View = world.GetPool<BaseViewComponent>();
            Dir = world.GetPool<Direction>();
            Speed = world.GetPool<Speed>();
            Hp = world.GetPool<Hp>();
            Anim = world.GetPool<AnimationComponent>();
            Inventory = world.GetPool<Inventory>();
            CantMove = world.GetPool<CantMoveTag>();
            Dead = world.GetPool<DeadTag>();
            Rb = world.GetPool<RigidbodyComponent>();
            SlingUps = world.GetPool<SlingUps>();
            Level = world.GetPool<Level>();
            Element = world.GetPool<Element>();
            ModelChangerComponent = world.GetPool<ModelChangerComponent>();
            Radius = world.GetPool<Radius>();
            Enemy = world.GetPool<Enemy>();
            Through = world.GetPool<Through>();
            Ricochet = world.GetPool<Ricochet>();
            Obstacle = world.GetPool<Obstacle>();
            Ammo = world.GetPool<Ammo>();
            MaxSpeed = world.GetPool<MaxSpeed>();
            ReloadTick = world.GetPool<ReloadTick>();
            EnemySpawnTick = world.GetPool<EnemySpawnTick>();
            WaveDelayTick = world.GetPool<WaveDelayTick>();
            Stage = world.GetPool<Stage>();
            NavMeshAgentComponent = world.GetPool<NavMeshAgentComponent>();
            Exp = world.GetPool<Exp>();
            Damage = world.GetPool<Damage>();
            Knockback = world.GetPool<Knockback>();
            Tomes = world.GetPool<Tomes>();
            Mult = world.GetPool<Mult>();
            Elements = world.GetPool<Elements>();
            Fire = world.GetPool<Fire>();
            Explosive = world.GetPool<Explosive>();
            Lightning = world.GetPool<Lightning>();
            Slime = world.GetPool<Slime>();
            PrevHitTargets = world.GetPool<PrevHitTargets>();
            KnockedTick = world.GetPool<KnockedTick>();
            BurnTick = world.GetPool<BurnTick>();
            HpViewComponent = world.GetPool<HpViewComponent>();
            HitVFXProviderComponent = world.GetPool<HitVFXProviderComponent>();
            Lifetime = world.GetPool<Lifetime>();
            ZoneTriggers = world.GetPool<ZoneTriggers>();
            Zone = world.GetPool<Zone>();
            ZoneSpread = world.GetPool<ZoneSpread>();
            Ult = world.GetPool<Ult>();
            Weakness = world.GetPool<Weakness>();
            KnockbackWave = world.GetPool<KnockbackWave>();
            DeathAnimTick = world.GetPool<DeathAnimTick>();



            //events
            HitEvent = eventWorld.GetPool<HitEvent>();
            ShotCancelEvent = eventWorld.GetPool<ShotCancelEvent>();
            ShotEvent = eventWorld.GetPool<ShotEvent>();
            SpawnBallEvent = eventWorld.GetPool<SpawnBallEvent>();
            DamageEvent = eventWorld.GetPool<DamageEvent>();
            HitPlayerEvent = eventWorld.GetPool<HitPlayerEvent>();
            SpawnZoneEvent = eventWorld.GetPool<SpawnZoneEvent>();
            SetOnFireEvent = eventWorld.GetPool<SetOnFireEvent>();
            ZoneExitEvent = eventWorld.GetPool<ZoneExitEvent>();
            ZoneEnterEvent = eventWorld.GetPool<ZoneEnterEvent>();
            VFXEvent = eventWorld.GetPool<VFXEvent>();
            KnockbackEvent = eventWorld.GetPool<KnockbackEvent>();
        }


       
    }
}