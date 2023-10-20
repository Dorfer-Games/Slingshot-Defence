using Leopotam.EcsLite;
using Source.Scripts.System.Util;
using UnityEngine;

namespace Kuhpik
{
    public abstract class GameSystem : MonoBehaviour, IGameSystem
    {
        protected SaveData save;
        protected GameConfig config;
        protected GameData game;
        
        protected EcsWorld world => game.World;
        protected EcsWorld eventWorld => game.EventWorld;
        protected Pools pool => game.Pools;

        public virtual void OnCustomTick() { }

        public virtual void OnFixedUpdate() { }

        public virtual void OnGameEnd() { }

        public virtual void OnGameStart() { }

        public virtual void OnInit() { }

        public virtual void OnLateUpdate() { }

        public virtual void OnStateEnter() { }

        public virtual void OnStateExit() { }

        public virtual void OnUpdate() { }
    }
}