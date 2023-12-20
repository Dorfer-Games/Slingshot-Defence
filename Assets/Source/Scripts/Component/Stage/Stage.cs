﻿using System.Collections.Generic;
using Source.Scripts.Data;
using UnityEngine;

namespace Source.Scripts.Component.Battle
{
    public struct Stage
    {
        public int StartReward;
        public int RewardPerWave;
        
        public List<Wave> Waves;
        public List<int> AliveEnemies;
        public int CurrentWaveId;
        public int CurrentWaveEnemiesSpawnedCount;
        public Transform[] SpawnPositions;
        public bool UseCommonSpawnDelay;
        public float CommonSpawnDelay;

        public Wave CurrentWave => Waves[CurrentWaveId];
        public bool AllEnemiesSpawned => CurrentWaveEnemiesSpawnedCount == CurrentWave.Enemies.Count;
        public bool AllWavesComplete => CurrentWaveId == Waves.Count;
        public bool AllEnemiesDead => AliveEnemies.Count == 0 && AllEnemiesSpawned;
    }
}