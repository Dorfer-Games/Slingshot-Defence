using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.Movement;
using Source.Scripts.Component.ViewComponent;

namespace Source.Scripts.System.Move
{
    public class ReachSystem : GameSystem
    {
        private EcsFilter filterFollowPos;

        public override void OnInit()
        {
            base.OnInit();
            filterFollowPos = world.Filter<Direction>().Inc<Speed>().Inc<BaseViewComponent>().Inc<RigidbodyComponent>().Inc<FollowPosition>().Exc<CantMoveTag>().End();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            foreach (var ent in filterFollowPos)
            {
                var targetPos = pool.FollowPosition.Get(ent).Value;
                var entPos = pool.View.Get(ent).Value.transform.position;
                entPos.y = 0;
                targetPos.y = 0;
                if ((targetPos - entPos).sqrMagnitude < 1f)
                {
                    pool.FollowPosition.Del(ent);
                    //pool.Speed.Del(ent);
                    pool.ReachEvent.Add(eventWorld.NewEntity()).Entity = ent;
                }
               
            }
        }
    }
}