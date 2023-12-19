using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.Movement;


namespace Source.Scripts.System.Move
{
    public class TargetMoveSystem : GameSystem
    {
        private EcsFilter filterFollowPos;

        public override void OnInit()
        {
            base.OnInit();
            filterFollowPos = world.Filter<Direction>().Inc<Speed>().Inc<RigidbodyComponent>().Inc<FollowPosition>().Exc<CantMoveTag>().End();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            
            foreach (var ent in filterFollowPos)
            {
                ref var dir = ref pool.Dir.Get(ent).Value;
                var targetPos = pool.FollowPosition.Get(ent).Value;
                var entTr = pool.View.Get(ent).Value.transform;
                var newDir = (targetPos - entTr.position).normalized;
                dir = newDir;
            }
        }
    }
}