using System.Collections.Generic;
using DG.Tweening;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.Battle.Ball;
using Source.Scripts.Component.Event;
using Source.Scripts.Data.Enum;
using Source.Scripts.View.VFX;
using UnityEngine;

namespace Source.Scripts.System.Battle
{
    public class HitSystem : GameSystem
    {
        private EcsFilter filter;

        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<HitEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            var list = new List<int>();
            foreach (var e in filter)
            {
                var hitEvent = pool.HitEvent.Get(e);
                var sender = hitEvent.Sender;
                var targets = hitEvent.Targets;

                if (list.Contains(sender))
                    continue;

                if (!DetectHit(sender, targets))
                    continue;


                if (pool.Ricochet.Has(sender) && pool.Through.Has(sender))
                {
                    HandleThroughRicochet(sender);
                }
                else if (pool.Ricochet.Has(sender))
                {
                    HandleRicochet(sender);
                }
                else if (pool.Through.Has(sender))
                {
                    HandleThrough(sender);
                }
                else
                {
                    pool.Dead.GetOrCreateRef(sender);
                }

                var elementType = pool.Element.Get(sender).Value;
                var vfxId = (int) elementType;
                if (pool.Ult.Has(sender))
                    vfxId += config.ElementsCount - 1;
                var baseDamage = pool.Damage.Get(sender).Value;
                foreach (var target in targets)
                {
                    bool sendVFX = true;

                    SendDamage(sender, target, baseDamage);
                    //proc element
                    if (pool.Fire.Has(sender))
                    {
                        ProcFire(sender, target, baseDamage);
                    }

                    if (pool.Explosive.Has(sender))
                    {
                        //each explodes
                        ProcExplosion(sender, target, baseDamage, vfxId);
                    }

                    if (pool.Lightning.Has(sender))
                    {
                        sendVFX = false;
                        if (pool.Ult.Has(sender))
                            ProcLightningUlt(sender, target, baseDamage, vfxId);
                        else
                            ProcLightning(sender, target, targets, baseDamage);
                    }

                    if (pool.KnockbackWave.Has(sender))
                    {
                        ProcKnockbackWave(sender, target);
                    }

                    if (pool.ZoneSpread.Has(sender))
                    {
                        ProcZoneSpread(sender, target);
                    }

                    if (sendVFX)
                    {
                        SendVFX(vfxId, target);
                    }
                }


                list.Add(sender);
            }

            list.Clear();
        }

        private void ProcKnockbackWave(int sender, int target)
        {
            var knockbackWave = pool.KnockbackWave.Get(sender);
            var enemiesInRadius = game.PositionService.GetBackEnemiesInRadius(sender, target, knockbackWave.Radius);
            foreach (var en in enemiesInRadius)
            {
                ref var knockbackEvent = ref pool.KnockbackEvent.Add(eventWorld.NewEntity());
                knockbackEvent.Sender = sender;
                knockbackEvent.Target = en;
                knockbackEvent.Force = pool.Knockback.Get(sender).Value;
            }
            //cast vfx here if needed to rescale
        }

        private void ProcFire(int sender, int target, float baseDamage)
        {
            ref var setOnFireEvent = ref pool.SetOnFireEvent.Add(eventWorld.NewEntity());
            setOnFireEvent.Sender = sender;
            setOnFireEvent.Target = target;
            setOnFireEvent.Damage = baseDamage;
        }

        private void ProcExplosion(int sender, int target, float baseDamage, int vfxId)
        {
            ref var explosive = ref pool.Explosive.Get(sender);
            var enemiesInRadius = game.PositionService.GetEnemiesInRadius(target, explosive.ExplosionRadius);
            float damage = baseDamage * explosive.ExplosionDamagePercent / 100f;
            foreach (var explosionEnt in enemiesInRadius)
            {
                SendDamage(sender, explosionEnt, damage);
                if (pool.Weakness.Has(sender))
                {
                    ref var weakness = ref pool.Weakness.GetOrCreateRef(explosionEnt);
                    weakness = pool.Weakness.Get(sender);
                    SendVFX(vfxId, explosionEnt);
                }
            }

            SendVFX((int) ElementType.DARKNESS, target);
        }

        private void ProcLightningUlt(int sender, int target, float baseDamage, int vfxId)
        {
            var lightning = pool.Lightning.Get(sender);
            var rndEnemies =
                game.PositionService.GetRandomEnemiesInRadius(target, lightning.Radius, lightning.TargetsCount);
            foreach (var rndEnemy in rndEnemies)
            {
                SendDamage(sender, rndEnemy, baseDamage * lightning.LightningDamagePercent / 100f);
                SendVFX(vfxId, target);
            }
        }

        private void ProcLightning(int sender, int target, List<int> targets, float baseDamage)
        {
            ref var lightning = ref pool.Lightning.Get(sender);
            var lightingTargets = game.PositionService.GetClosestSequence(target, lightning.Radius,
                lightning.TargetsCount,
                targets);

            float damage = baseDamage * lightning.LightningDamagePercent / 100f;
            foreach (var lightingTarget in lightingTargets)
            {
                SendDamage(sender, lightingTarget, damage);
            }

            for (int i = 0; i < lightingTargets.Count - 1; i++)
            {
                int fromE = lightingTargets[i];
                int toE = lightingTargets[i + 1];
                var hitVFXProviderView = pool.HitVFXProviderComponent.Get(fromE).Value;
                var vfx = (LightningVFX) hitVFXProviderView.VFXs[(int) ElementType.LIGHTNING];
                var fromPos = pool.View.Get(fromE).Value.transform.position;
                var toPos = pool.View.Get(toE).Value.transform.position;
                var dir = toPos - fromPos;

                vfx.transform.rotation = Quaternion.LookRotation(dir);

                vfx.SetLength(dir.magnitude);
                vfx.gameObject.SetActive(true);
            }
        }

        private void ProcZoneSpread(int sender, int target)
        {
            ref var zoneSpread = ref pool.ZoneSpread.Get(sender);
            var zoneSpreadRadius = zoneSpread.SpreadRadius;
            for (int i = 0; i < zoneSpread.Count; i++)
            {
                Vector3 pos = pool.View.Get(target).Value.transform.position;

                if (i > 0)
                {
                    var rndOffset = new Vector3(Random.Range(-zoneSpreadRadius, zoneSpreadRadius),
                        0, Random.Range(-zoneSpreadRadius, zoneSpreadRadius));
                    pos += rndOffset;
                    
                    int zoneE=SpawnZone(sender, pos);
                    ZoneSpreadAnim(zoneE, sender, target);
                }
                else
                {
                    int zoneE=SpawnZone(sender, pos);
                    pool.SpawnZoneEvent.Add(eventWorld.NewEntity()).Entity = zoneE;
                }

              
            }
        }

        private void ZoneSpreadAnim(int zoneE, int sender,int target)
        {
            var zoneGo = pool.View.Get(zoneE).Value.gameObject;
            zoneGo.SetActive(false);

            var fromPos = pool.View.Get(target).Value.transform.position;
            var toPos = pool.View.Get(zoneE).Value.transform.position;

            var elementType = pool.Element.Get(sender).Value;
            var zoneSpread = Instantiate(config.ZoneSpreadPrefabs[elementType]);
            zoneSpread.transform.localPosition = fromPos;

            DOTween.Sequence()
                .Append(zoneSpread.transform.DOJump(toPos, 3f, 1, 0.3f).SetEase(Ease.Linear))
                .AppendCallback(() =>
                {
                    Destroy(zoneSpread.gameObject);
                    zoneGo.SetActive(true);
                    //event
                    pool.SpawnZoneEvent.Add(eventWorld.NewEntity()).Entity = zoneE;
                });
        }

        private void SendVFX(int vfxId, int target)
        {
            ref var vfxEvent = ref pool.VFXEvent.Add(eventWorld.NewEntity());
            vfxEvent.VFXId = vfxId;
            vfxEvent.Target = target;
            vfxEvent.Toggle = true;
        }

        private void SendDamage(int sender, int target, float damage)
        {
            ref var damageEvent = ref pool.DamageEvent.Add(eventWorld.NewEntity());
            damageEvent.Sender = sender;
            damageEvent.Target = target;
            damageEvent.Damage = damage;
        }

        private int SpawnZone(int sender, Vector3 pos)
        {
            var elementType = pool.Element.Get(sender).Value;
            ref var zoneSpread = ref pool.ZoneSpread.Get(sender);
            var radius = zoneSpread.ZoneRadius / config.ScaleFactor;

            var zoneView = Instantiate(config.ZonesPrefabs[elementType]);
            var zoneT = zoneView.transform;
            zoneT.position = pos;
            zoneT.localScale = new Vector3(radius, 1, radius);
            zoneT.rotation=Quaternion.Euler(0,Random.Range(0f,360f),0);

            var zoneE = game.Fabric.InitView(zoneView);

            ref var zone = ref pool.Zone.Add(zoneE);
            zone.Time = zoneSpread.Time;
            zone.Radius = zoneSpread.ZoneRadius;

            if (pool.Slime.Has(sender))
            {
                ref var slime = ref pool.Slime.Get(sender);
                ref var slimeZone = ref pool.Slime.Add(zoneE);
                slimeZone = slime;
            }

            if (pool.Fire.Has(sender))
            {
                ref var fire = ref pool.Fire.Get(sender);
                ref var fireZone = ref pool.Fire.Add(zoneE);
                fireZone = fire;

                pool.Damage.Add(zoneE).Value = pool.Damage.Get(sender).Value;
            }

            return zoneE;
        }

        private bool DetectHit(int sender, List<int> targets)
        {
            bool detect = false;

            var hashSet = UnpackTargets(sender);
            //has new target
            foreach (var target in targets)
            {
                if (!hashSet.Contains(target))
                {
                    detect = true;
                    hashSet.Clear();
                    hashSet.UnionWith(targets);
                    break;
                }
            }

            //update prev hit targets
            HashSet<EcsPackedEntity> newSet = new();
            foreach (var ent in hashSet)
            {
                newSet.Add(world.PackEntity(ent));
            }

            pool.PrevHitTargets.Get(sender).Value = newSet;

            return detect;
        }

        private void HandleThrough(int sender)
        {
            ref var through = ref pool.Through.Get(sender);
            if (through.Value <= 0)
                pool.Dead.Add(sender);
            else
                through.Value--;
        }

        private void HandleThroughRicochet(int sender)
        {
            ref var through = ref pool.Through.Get(sender);
            if (through.Value <= 0)
            {
                pool.Through.Del(sender);
                HandleRicochet(sender);
            }
            else
                through.Value--;
        }

        private void HandleRicochet(int sender)
        {
            ref var ricochet = ref pool.Ricochet.Get(sender);

            if (ricochet.Count <= 0)
            {
                pool.Dead.Add(sender);
            }
            else
            {
                ricochet.Count--;
                var hitTargets = UnpackTargets(sender);
                var newTargets =
                    game.PositionService.GetEnemiesInRadiusWithPriority(sender, ricochet.Radius, hitTargets,
                        true);
                if (newTargets.Count > 0)
                {
                    var rnd = Random.Range(0, newTargets.Count);
                    var newTarget = newTargets[rnd];
                    var targetPos = pool.View.Get(newTarget).Value.transform.position;
                    var senderPos = pool.View.Get(sender).Value.transform.position;
                    var dir = (targetPos - senderPos).normalized;
                    dir.y = 0;
                    pool.Dir.Get(sender).Value = dir;
                }
                else
                {
                    pool.Dead.Add(sender);
                }
            }
        }

        private HashSet<int> UnpackTargets(int sender)
        {
            var hitTargets = pool.PrevHitTargets.Get(sender).Value;
            var hashSet = new HashSet<int>();
            foreach (var packedEntity in hitTargets)
            {
                if (packedEntity.Unpack(world, out int entity))
                    hashSet.Add(entity);
            }

            return hashSet;
        }
    }
}