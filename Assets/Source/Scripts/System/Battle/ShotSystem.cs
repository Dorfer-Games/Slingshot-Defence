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

                var ent = SpawnBall();

                //ammo
                var firstAmmo = ammo.Value[^ammo.Count];
                pool.View.Get(firstAmmo).Value.ToggleActive(false);
                ammo.Count--;
                
                //add ball perks
                var elementType = pool.Element.Get(firstAmmo).Value;
                var tomes = pool.Tomes.Get(game.PlayerEntity).Value;
                List<int> ballsInShot = new List<int>(){ent};
                foreach (var tomeLvl in tomes)
                {
                    if (tomeLvl.Value==0)
                        continue;

                    switch (tomeLvl.Key)
                    {
                        case TomeType.THROUGH:
                            pool.Through.Add(ent).Value=config.ThroughTome[tomeLvl.Value].Value;
                            break;
                        case TomeType.RADIUS:
                            pool.Radius.Get(ent).Value=config.RadiusTome[tomeLvl.Value].Value*config.ScaleFactor;
                            break;
                        case TomeType.RICOCHET:
                            ref var ricochet = ref pool.Ricochet.Add(ent);
                            ricochet= config.RicochetTome[tomeLvl.Value];
                            break;
                        case TomeType.KNOCKBACK:
                            pool.Knockback.Get(ent).Value=config.KnockbackTome[tomeLvl.Value].Value;
                            break;
                        case TomeType.MULT:
                          
                            var mult=config.MultTome[tomeLvl.Value];
                            
                            var ball1Tr = pool.View.Get(ent).Value.transform;
                            var scaleX = ball1Tr.localScale.x;
                            if (mult.AddBallCount==1)
                            {
                                MoveLocal(ent, new Vector3(-scaleX / 2f, 0, 0));
                                
                                var addBall = SpawnBall();
                                ballsInShot.Add(addBall);
                                MoveLocal(addBall, new Vector3(scaleX / 2f, 0, 0));
                            }else if (mult.AddBallCount==2)
                            {
                                var addBall1 = SpawnBall();
                                ballsInShot.Add(addBall1);
                                MoveLocal(addBall1, new Vector3(-scaleX * 1.5f, 0, 0));
                                RotateLocal(addBall1, -config.MultishotAngle);
                                var addBall2 = SpawnBall();
                                ballsInShot.Add(addBall2);
                                MoveLocal(addBall2, new Vector3(scaleX *1.5f, 0, 0));
                                RotateLocal(addBall2, config.MultishotAngle);
                            }

                            foreach (var multBall in ballsInShot)
                            {
                                pool.Damage.Get(multBall).Value *= (mult.DamagePercent / 100f);
                            }
                           
                            break;
                    }
                }

                var ballLevel = pool.Elements.Get(game.PlayerEntity).Value[elementType];
                ref var damage = ref pool.Damage.Get(ent).Value;
                int addDamagePercent = 0;
                foreach (var ballE in ballsInShot)
                {
                    switch (elementType)
                    {
                        case ElementType.FIRE:
                            ref var fire = ref pool.Fire.Add(ballE);
                            fire = config.FireBall[ballLevel];
                            addDamagePercent = fire.AddDamagePercent;
                            break;
                        case ElementType.DARKNESS:
                            ref var dark = ref pool.Dark.Add(ballE);
                            dark = config.DarkBall[ballLevel];
                            dark.ExplosionRadius *= config.ScaleFactor;
                            addDamagePercent = dark.AddDamagePercent;
                            break;
                        case ElementType.LIGHTNING:
                            ref var lightning = ref pool.Lightning.Add(ballE);
                            lightning = config.LightningBall[ballLevel];
                            lightning.Radius *= config.ScaleFactor;
                            addDamagePercent = lightning.AddDamagePercent;
                            break;
                        case ElementType.BOULDER:
                            ref var boulder = ref pool.Boulder.Add(ballE);
                            boulder = config.BoulderBall[ballLevel];
                            pool.Knockback.Get(ballE).Value += boulder.AddKnockbackForce;
                            addDamagePercent = boulder.AddDamagePercent;
                            break;
                        case ElementType.SLIME:
                            ref var slime = ref pool.Slime.Add(ballE);
                            slime = config.SlimeBall[ballLevel];
                            slime.SlowRadius *= config.ScaleFactor;
                            addDamagePercent = slime.AddDamagePercent;
                            break;
                    }

                    damage += damage * addDamagePercent / 100f;

                    //set model
                    pool.ModelChangerComponent.Get(ballE).Value.SetModel((int) elementType);
                }
            }
        }

        private void RotateLocal(int ent, float degree)
        {
            var playerViewTransform = game.PlayerView.transform;
            var ballTr = pool.View.Get(ent).Value.transform;
            ballTr.SetParent(playerViewTransform);
            ballTr.localRotation=Quaternion.Euler( ballTr.localRotation.x,degree, ballTr.localRotation.z);
            pool.Dir.Get(ent).Value = ballTr.forward;
            ballTr.SetParent(null);
        }

        private void MoveLocal(int ent,Vector3 offset)
        {
            var playerViewTransform = game.PlayerView.transform;
            var ballTr = pool.View.Get(ent).Value.transform;
            ballTr.SetParent(playerViewTransform);
            ballTr.localPosition +=offset;
            ballTr.SetParent(null);
        }

        private int SpawnBall()
        {
            var ball = Instantiate(config.BallPrefab);
            ball.transform.position = game.PlayerView.BallSpawnPos.transform.position;
            var ent = game.Fabric.InitView(ball);
            pool.Dir.Get(ent).Value = game.PlayerView.transform.forward;
            pool.Speed.Add(ent).Value = config.BallSpeed;
            pool.PrevHitTargets.Add(ent).Value = new ();
            pool.Radius.Add(ent).Value = config.BallBaseRadius*config.ScaleFactor;
            pool.Knockback.Add(ent).Value = config.BallBaseKnockback;
            //set base damage
            var damageLevel = save.SlingUps[save.CurrentSling][UpType.DAMAGE];
            pool.Damage.Add(ent).Value = config.SlingConfigs[save.CurrentSling].Ups[UpType.DAMAGE][damageLevel];
            
            pool.SpawnBallEvent.Add(eventWorld.NewEntity()).Value = ent;
            return ent;
        }
    }
}