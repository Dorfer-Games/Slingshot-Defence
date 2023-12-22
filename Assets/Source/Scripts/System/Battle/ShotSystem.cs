using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.Event;
using Source.Scripts.Data.Enum;
using UnityEngine;

namespace Source.Scripts.System.Sling.Shot
{
    public class ShotSystem : GameSystem
    {
        private EcsFilter filter;

        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<ShotEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filter)
            {
                ref var ammo = ref pool.Ammo.Get(game.PlayerEntity);
                if (ammo.Count == 0)
                    return;

                //ammo
                var firstAmmo = ammo.Value[^ammo.Count];
                pool.View.Get(firstAmmo).Value.ToggleActive(false);
                ammo.Count--;

                var shotLen = pool.ShotEvent.Get(e).ShotLen;
                var isCrit= pool.ShotEvent.Get(e).IsCrit;
                var ent = SpawnBall(firstAmmo,shotLen,isCrit);
                RotateLocal(ent, 0,shotLen);
                if (pool.Ult.Has(firstAmmo))
                {
                    InitUlt(ent,firstAmmo);
                    return;
                }
              
                var ballsInShot = InitTomes(ent,firstAmmo,shotLen,isCrit);
                InitBallsElement(ballsInShot,firstAmmo);
            }
        }

        private void InitUlt(int ent,int firstAmmo)
        {
            var ultType = pool.Ult.Get(firstAmmo).Value;
            switch (ultType)
            {
                case UltType.METEOR:
                    ref var fire = ref pool.Fire.GetOrCreateRef(ent);
                    fire = config.MeteorUlt.Fire;
                    ref var zoneSpread = ref pool.ZoneSpread.GetOrCreateRef(ent);
                    zoneSpread = config.MeteorUlt.ZoneSpread;
                    zoneSpread.ZoneRadius*= config.ScaleFactor;
                    break;
                case UltType.SUPER_DARKNESS:
                    ref var explosive = ref pool.Explosive.GetOrCreateRef(ent);
                    explosive = config.SuperDarkUlt.Explosive;
                    explosive.ExplosionRadius *= config.ScaleFactor;
                    
                    ref var weakness = ref pool.Weakness.GetOrCreateRef(ent);
                    weakness = config.SuperDarkUlt.Weakness;
                    break;
                case UltType.THUNDER_BALL:
                    ref var lightning = ref pool.Lightning.GetOrCreateRef(ent);
                    lightning = config.ThunderBallUlt.Lightning;
                    lightning.Radius *= config.ScaleFactor;
                    break;
                case UltType.SUPER_BOULDER:
                    pool.Knockback.GetOrCreateRef(ent).Value = config.BallBaseKnockback + config.SuperBoulderUlt.BoulderData.AddKnockbackForce;
                    ref var knockbackWave = ref pool.KnockbackWave.GetOrCreateRef(ent);
                    knockbackWave = config.SuperBoulderUlt.KnockbackWave;
                    knockbackWave.Radius *= config.ScaleFactor;
                    break;
                case UltType.SUPER_SLIME:
                    ref var slime = ref pool.Slime.GetOrCreateRef(ent);
                    slime = config.SuperSlimeUlt.Slime;
                    ref var zoneSpreadSlime = ref pool.ZoneSpread.GetOrCreateRef(ent);
                    zoneSpreadSlime = config.SuperSlimeUlt.ZoneSpread;
                    zoneSpreadSlime.ZoneRadius*= config.ScaleFactor;
                    break;
            }
        }

        private List<int> InitTomes(int ent,int firstAmmo,float shotLen,bool isCrit)
        {
            //add ball perks
            var tomes = pool.Tomes.Get(game.PlayerEntity).Value;
            List<int> ballsInShot = new List<int>() {ent};

            if (tomes[TomeType.MULT]>0)
            {
                ref var mult = ref pool.Mult.Add(ent);
                mult = config.MultTome[tomes[TomeType.MULT]];
                var ball1Tr = pool.View.Get(ent).Value.transform;
                var scaleX = ball1Tr.localScale.x;
                if (mult.AddBallCount == 1)
                {
                    MoveLocal(ent, new Vector3(-scaleX / 2f, 0, 0));
                    RotateLocal(ent, 0,shotLen);
                    var addBall = SpawnBall(firstAmmo,shotLen,isCrit);
                    ballsInShot.Add(addBall);
                    MoveLocal(addBall, new Vector3(scaleX / 2f, 0, 0));
                    RotateLocal(addBall, 0,shotLen);
                }
                else if (mult.AddBallCount == 2)
                {
                    var addBall1 = SpawnBall(firstAmmo,shotLen,isCrit);
                    ballsInShot.Add(addBall1);
                    MoveLocal(addBall1, new Vector3(-scaleX * 1.5f, 0, 0));
                    RotateLocal(addBall1, -config.MultishotAngle,shotLen);

                    var addBall2 = SpawnBall(firstAmmo,shotLen,isCrit);
                    ballsInShot.Add(addBall2);
                    MoveLocal(addBall2, new Vector3(scaleX * 1.5f, 0, 0));
                    RotateLocal(addBall2, config.MultishotAngle,shotLen);
                }

                foreach (var multBall in ballsInShot)
                {
                    pool.Damage.Get(multBall).Value *= (mult.DamagePercent / 100f);
                }
            }
            
            foreach (var tomeLvl in tomes)
            {
                if (tomeLvl.Value == 0)
                    continue;

                switch (tomeLvl.Key)
                {
                    case TomeType.THROUGH:
                        pool.Through.Add(ent).Value = config.ThroughTome[tomeLvl.Value].Value;
                        break;
                    case TomeType.RADIUS:
                        pool.Radius.Get(ent).Value = config.RadiusTome[tomeLvl.Value].Value * config.ScaleFactor;
                        break;
                    case TomeType.RICOCHET:
                        ref var ricochet = ref pool.Ricochet.Add(ent);
                        ricochet = config.RicochetTome[tomeLvl.Value];
                        break;
                    case TomeType.KNOCKBACK:
                        SetKnockback(ent,firstAmmo);
                        break;
                    case TomeType.MULT:
                        break;
                }
            }

            return ballsInShot;
        }

        private void InitBallsElement(List<int> ballsInShot,int firstAmmo)
        {
            var ballLevel = pool.Level.Get(firstAmmo).Value;
            var elementType = pool.Element.Get(firstAmmo).Value;
            //var ballLevel = pool.Elements.Get(game.PlayerEntity).Value[elementType];
          
            foreach (var ballE in ballsInShot)
            {
                switch (elementType)
                {
                    case ElementType.FIRE:
                        ref var fire = ref pool.Fire.Add(ballE);
                        fire = config.FireBall[ballLevel].Fire;
                        break;
                    case ElementType.DARKNESS:
                        ref var explosive = ref pool.Explosive.Add(ballE);
                        explosive = config.DarkBall[ballLevel].Explosive;
                        explosive.ExplosionRadius *= config.ScaleFactor;
                        break;
                    case ElementType.LIGHTNING:
                        ref var lightning = ref pool.Lightning.Add(ballE);
                        lightning = config.LightningBall[ballLevel].Lightning;
                        lightning.Radius *= config.ScaleFactor;
                        break;
                    case ElementType.BOULDER:
                        var addKnockback = config.BoulderBall[ballLevel].AddKnockbackForce;
                        pool.Knockback.Get(ballE).Value += addKnockback;
                        break;
                    case ElementType.SLIME:
                        ref var slime = ref pool.Slime.Add(ballE);
                        var slimeData = config.SlimeBall[ballLevel];
                        slime = slimeData.Slime;
                        ref var zoneSpread = ref pool.ZoneSpread.Add(ballE);
                        zoneSpread = slimeData.ZoneSpread;
                        zoneSpread.ZoneRadius *= config.ScaleFactor;
                        break;
                }
            }
        }

        private void SetKnockback(int ballE,int firstAmmo)
        {
            float value = 0f;
            var tomes = pool.Tomes.Get(game.PlayerEntity).Value;
            var elementType = pool.Element.Get(firstAmmo).Value;
            var ammoLvl = pool.Level.Get(firstAmmo).Value;
            if (elementType==ElementType.BOULDER && ammoLvl>0)
            {
                if (ammoLvl > 4)
                {
                    value += config.SuperBoulderUlt.BoulderData.AddKnockbackForce;
                }
                else
                    value += config.BoulderBall[ammoLvl].AddKnockbackForce;
            }

            if (tomes[TomeType.KNOCKBACK]>0)
            {
                value += config.KnockbackTome[tomes[TomeType.KNOCKBACK]].Value;
            }
            else
            {
                value += config.BallBaseKnockback;
            }

            pool.Knockback.Get(ballE).Value = value;
        }

        private void AddDamagePercent(int ent,int firstAmmo)
        {
            var elementType = pool.Element.Get(firstAmmo).Value;
            var ballLevel = pool.Level.Get(firstAmmo).Value;
            ref var damage = ref pool.Damage.Get(ent).Value;
            int addDamagePercent = config.AddDamagePercentProg[elementType][ballLevel];
            damage += damage * addDamagePercent / 100f;
        }

        private void RotateLocal(int ent, float degree,float shotLen)
        {
            var playerViewTransform = game.PlayerView.transform;
            var ballTr = pool.View.Get(ent).Value.transform;
            ballTr.SetParent(playerViewTransform);
            ballTr.localRotation = Quaternion.Euler(ballTr.localRotation.x, degree, ballTr.localRotation.z);
            pool.Dir.Get(ent).Value = ballTr.forward;
            pool.FollowPosition.Get(ent).Value = ballTr.position+shotLen * ballTr.forward;
            ballTr.SetParent(null);
        }

        private void MoveLocal(int ent, Vector3 offset)
        {
            var playerViewTransform = game.PlayerView.transform;
            var ballTr = pool.View.Get(ent).Value.transform;
            ballTr.SetParent(playerViewTransform);
            ballTr.localPosition += offset;
            ballTr.SetParent(null);
        }

        private int SpawnBall(int firstAmmo,float shotLen,bool isCrit)
        {
            var elementType = pool.Element.Get(firstAmmo).Value;
            var speed = (shotLen / config.StageMaxZ) * config.BallSpeed;
            var dir = game.PlayerView.transform.forward;

            var ball = Instantiate(config.BallPrefab);
            ball.transform.position = game.PlayerView.BallSpawnPos.transform.position;
            var ent = game.Fabric.InitView(ball);
            pool.Dir.Get(ent).Value = dir;
            pool.Speed.Add(ent).Value = speed;
            pool.PrevHitTargets.Add(ent).Value = new();
            pool.Radius.Add(ent).Value = config.BallBaseRadius * config.ScaleFactor;
            pool.Knockback.Add(ent).Value = config.BallBaseKnockback;
            pool.Element.Add(ent).Value = elementType;
            pool.FollowPosition.Add(ent).Value = shotLen * dir;
            //set base damage
            var damageLevel = save.SlingUps[save.CurrentSling][UpType.DAMAGE];
            var baseDamage = config.SlingConfigs[save.CurrentSling].Ups[UpType.DAMAGE][damageLevel];
            if (isCrit)
            {
                var critLvl = save.SlingUps[save.CurrentSling][UpType.CRIT_K];
                var critK = config.SlingConfigs[save.CurrentSling].Ups[UpType.CRIT_K][critLvl];
                baseDamage *= critK;
            }
            pool.Damage.Add(ent).Value = baseDamage;
            AddDamagePercent(ent, firstAmmo);
           
            int modelID = (int) elementType;
            if (pool.Ult.Has(firstAmmo))
            {
                pool.Ult.Add(ent).Value = (UltType)elementType;
                modelID += config.ElementsCount-1;
            }
            //set model
            pool.ModelChangerComponent.Get(ent).Value.SetModel(modelID);
            
            //event
            pool.SpawnBallEvent.Add(eventWorld.NewEntity()).Value = ent;
            return ent;
        }
    }
}