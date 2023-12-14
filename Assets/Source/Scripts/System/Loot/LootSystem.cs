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
            int expValue=0;
            int goldValue=0;
            foreach (var ent in filter)
            {
                expValue += pool.Exp.Get(ent).Value;
                goldValue += pool.Inventory.Get(ent).Value[ResType.GOLD];
            }

            if (expValue>0)
            {
                if (config.ExpProgression.Length>game.LvlId+1)
                {
                    var sum = game.CurExp + expValue;
                    if (sum >= config.ExpProgression[game.LvlId + 1])
                    {
                        sum -= config.ExpProgression[game.LvlId + 1];
                        game.LvlId++;
                        game.CurExp = sum;
                        pool.LvlUpEvent.Add(eventWorld.NewEntity());
                    }
                    else
                        game.CurExp = sum;
                    
                    SetExp();
                }
                else
                {
                    screen.SetMaxExp();
                }

            }
            
            if (goldValue>0)
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
            screen.SetExp(game.CurExp,config.ExpProgression[game.LvlId+1]);
        }
    }
}