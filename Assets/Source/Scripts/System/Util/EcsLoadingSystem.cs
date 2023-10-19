using Kuhpik;
using Source.Scripts.Service;
using Source.Scripts.System.Util;

namespace Source.Scripts.System
{
    public class EcsLoadingSystem : GameSystem
    {
        public override void OnInit()
        {
            base.OnInit();
            game.Pools = new Pools(game.World,game.EventWorld);
            game.Fabric = new Fabric(game.World,save,game,config,game.Pools);
            game.PositionService = new PositionService(game.World, save, game, config, game.Pools);
        }
    }
}