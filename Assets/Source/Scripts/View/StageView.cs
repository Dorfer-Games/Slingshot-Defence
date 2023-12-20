using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Source.Scripts.Data;
using Source.Scripts.Data.Enum;
using UnityEngine;

namespace Source.Scripts.View
{
    public class StageView : MonoBehaviour
    {
        public int StartReward;
        public int RewardPerWave;
        public bool UseCommonSpawnDelay=true;
        [ShowIf("UseCommonSpawnDelay")]
        public float CommonSpawnDelay;
        public List<Wave> Waves;
        public Transform[] SpawnPositions;
    }
}