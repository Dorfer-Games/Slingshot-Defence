using System.Collections.Generic;
using Source.Scripts.Data;

namespace Source.Scripts.Component.Battle
{
    public struct Stage
    {
        public List<Wave> Waves;
        public List<int> AliveEnemies;
        public int CurrentWave;
    }
}