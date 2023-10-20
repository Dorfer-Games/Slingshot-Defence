﻿using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Battle;
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
        public readonly EcsPool<AnimatorComponent> Anim;
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
        public readonly EcsPool<Moveable> Moveable;
        public readonly EcsPool<ReloadTick> ReloadTick;
     

        //events
        public readonly EcsPool<HitEvent> HitEvent;
        public readonly EcsPool<ShotCancelEvent> ShotCancelEvent;
        public readonly EcsPool<ShotEvent> ShotEvent;
        public readonly EcsPool<SpawnBallEvent> SpawnBallEvent;

        public Pools(EcsWorld world, EcsWorld eventWorld)
        {
            View = world.GetPool<BaseViewComponent>();
            Dir = world.GetPool<Direction>();
            Speed = world.GetPool<Speed>();
            Hp = world.GetPool<Hp>();
            Anim = world.GetPool<AnimatorComponent>();
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
            Moveable = world.GetPool<Moveable>();
            ReloadTick = world.GetPool<ReloadTick>();
          
            
            //events
            HitEvent = eventWorld.GetPool<HitEvent>();
            ShotCancelEvent = eventWorld.GetPool<ShotCancelEvent>();
            ShotEvent = eventWorld.GetPool<ShotEvent>();
            SpawnBallEvent = eventWorld.GetPool<SpawnBallEvent>();
        }

       
    }
}