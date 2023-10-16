using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.Event;
using Source.Scripts.Component.Movement;
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
        public readonly EcsPool<Ups> Ups;

        //events
        public readonly EcsPool<HitEvent> HitEvent;

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
            Ups = world.GetPool<Ups>();


            //events
            HitEvent = eventWorld.GetPool<HitEvent>();
        }

       
    }
}