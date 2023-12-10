using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Source.Scripts.Component.Battle
{
    public struct PrevHitTargets
    {
        public HashSet<EcsPackedEntity> Value;
    }
}