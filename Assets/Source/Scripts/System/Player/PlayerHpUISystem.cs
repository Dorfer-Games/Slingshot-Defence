using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.Event;
using Source.Scripts.View;

namespace Source.Scripts.System.Player
{
    public class PlayerHpUISystem : GameSystemWithScreen<GameUIScreen>
    {
        private EcsFilter filter;
        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<DamageEvent>().End();
            SetPlayerHp();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filter)
            {
                var target = pool.DamageEvent.Get(e).Target;
                if (target==game.PlayerEntity)
                {
                    SetPlayerHp();
                }
            }
        }

        private void SetPlayerHp()
        {
            var hp = pool.Hp.Get(game.PlayerEntity);
            screen.SetHp((int)hp.CurHp,(int)hp.MaxHp);
        }
    }
}