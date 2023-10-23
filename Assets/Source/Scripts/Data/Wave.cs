using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Source.Scripts.Data.Enum;
using UnityEngine;

namespace Source.Scripts.Data
{
    [Serializable]
    public struct Wave
    {
        public bool UseCommonSpawnDelay;
        public float CommonSpawnDelay;
        
        public float WaveDelay;
        public List<TypeLevelPair> Enemies; 
        
        [Serializable]
        public struct TypeLevelPair
        {
            public EnemyType EnemyType;
            public int Level;
        }
    }
}