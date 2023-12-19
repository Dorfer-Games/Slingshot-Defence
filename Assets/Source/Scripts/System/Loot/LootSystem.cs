using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Data.Enum;

namespace Source.Scripts.System.Loot
{
    public class LootSystem : GameSystemWithScreen<GameUIScreen>
    {
        private EcsFilter filter;

        public override void OnInit()
        {
            base.OnInit();
            filter = world.Filter<Enemy>().Inc<DeadTag>().Exc<DeathAnimTick>().End();

            //start ui
            SetGold();
            SetExp();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            int expValue = 0;
            int goldValue = 0;
            foreach (var ent in filter)
            {
                expValue += pool.Exp.Get(ent).Value;
                goldValue += pool.Inventory.Get(ent).Value[ResType.GOLD];
            }

            if (expValue > 0)
            {
                if (game.LvlId +1 == config.ExpProgression.Length)
                {
                    screen.SetMaxExp();
                }
                else
                {
                    var sum = game.CurExp + expValue;
                    while (sum >= config.ExpProgression[game.LvlId + 1])
                    {
                        sum -= config.ExpProgression[game.LvlId + 1];
                        game.LvlId++;
                        if (game.LvlId + 1 == config.ExpProgression.Length)
                            break;
                        else
                            pool.LvlUpEvent.Add(eventWorld.NewEntity());
                    }

                    game.CurExp = sum;
                    if (game.LvlId +1 != config.ExpProgression.Length)
                        SetExp();
                    else
                        screen.SetMaxExp();
                }
                
            }

            if (goldValue > 0)
            {
                save.PlayerInventory[ResType.GOLD] += goldValue;
                SetGold();
            }
        }

        private void SetGold()
        {
            screen.SetGold(save.PlayerInventory[ResType.GOLD]);
        }

        private void SetExp()
        {
            screen.SetExp(game.CurExp, config.ExpProgression[game.LvlId + 1]);
        }
    }
}