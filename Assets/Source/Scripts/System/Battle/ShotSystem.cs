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
                foreach (var tomeLvl in tomes)
                {
                    if (tomeLvl.Value==0)
                        continue;

                    switch (tomeLvl.Key)
                    {
                        case TomeType.TROUGH:
                            pool.Through.Add(ent).Value=config.ThroughTome[tomeLvl.Value].Value;
                            break;
                        case TomeType.RADIUS:
                            pool.Radius.Add(ent).Value=config.RadiusTome[tomeLvl.Value].Value;
                            break;
                        case TomeType.RICOCHET:
                            ref var ricochet = ref pool.Ricochet.Add(ent);
                            ricochet= config.RicochetTome[tomeLvl.Value];
                            break;
                        case TomeType.KNOCKBACK:
                            pool.Knockback.Get(ent).Value=config.KnockbackTome[tomeLvl.Value].Value;
                            break;
                        case TomeType.MULT:
                            //setup mult balls!!!!
                            ref var mult = ref pool.Mult.Get(ent);
                            mult.Value=config.MultTome[tomeLvl.Value].Value;
                            mult.DamagePercent=config.MultTome[tomeLvl.Value].DamagePercent;
                            break;
                    }
                }

                var ballLevel = pool.Elements.Get(game.PlayerEntity).Value[elementType];
                ref var damage = ref pool.Damage.Get(ent).Value;
                int addDamage = 0;
                switch (elementType)
                {
                    case ElementType.FIRE:
                        ref var fire = ref pool.Fire.Add(ent);
                        fire = config.FireBall[ballLevel];
                        addDamage = fire.AddDamagePercent;
                        break;
                    case ElementType.DARKNESS:
                        ref var dark = ref pool.Dark.Add(ent);
                        dark = config.DarkBall[ballLevel];
                        addDamage = dark.AddDamagePercent;
                        break;
                    case ElementType.LIGHTNING:
                        ref var lightning= ref pool.Lightning.Add(ent);
                        lightning = config.LightningBall[ballLevel];
                        addDamage = lightning.AddDamagePercent;
                        break;
                    case ElementType.BOULDER:
                        ref var boulder= ref pool.Boulder.Add(ent);
                        boulder = config.BoulderBall[ballLevel];
                        addDamage = boulder.AddDamagePercent;
                        break;
                    case ElementType.SLIME:
                        ref var slime= ref pool.Slime.Add(ent);
                        slime = config.SlimeBall[ballLevel];
                        addDamage = slime.AddDamagePercent;
                        break;
                }

                damage += damage * addDamage/100f;

                //set model
                pool.ModelChangerComponent.Get(ent).Value.SetModel((int)elementType);
            }
        }

        private int SpawnBall()
        {
            var ball = Instantiate(config.BallPrefab);
            ball.transform.position = game.PlayerView.BallSpawnPos.transform.position;
            var ent = game.Fabric.InitView(ball);
            pool.Dir.Get(ent).Value = game.PlayerView.transform.forward;
            pool.Speed.Add(ent).Value = config.BallSpeed;
            pool.TargetsHitCount.Add(ent).Value = new HashSet<int>();
            pool.Radius.Add(ent).Value = config.BallBaseRadius;
            pool.Knockback.Add(ent).Value = config.BallBaseKnockback;
            //set base damage
            var damageLevel = save.SlingUps[save.CurrentSling][UpType.DAMAGE];
            pool.Damage.Add(ent).Value = config.SlingConfigs[save.CurrentSling].Ups[UpType.DAMAGE][damageLevel];
            
            pool.SpawnBallEvent.Add(eventWorld.NewEntity()).Value = ent;
            return ent;
        }
    }
}