using System;
using AYellowpaper.SerializedCollections;
using Source.Scripts.Data.Enum;

namespace Source.Scripts.Data
{
    [Serializable]
    public struct Stats
    {
        public int Hp;
        public float Speed;
        public int Exp;
        public float SpawnDelay;
        public SerializedDictionary<ResType, int> Inventory;
    }
}