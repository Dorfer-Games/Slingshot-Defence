using System;

namespace Source.Scripts.Component.Battle
{
    [Serializable]
    public struct ZoneSpread
    {
        public float ZoneRadius;
        public float Time;
        public int Count;
        public float SpreadRadius;
    }
}