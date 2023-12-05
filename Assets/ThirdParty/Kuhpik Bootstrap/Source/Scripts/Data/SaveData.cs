﻿using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Source.Scripts.Data.Enum;

namespace Kuhpik
{
    /// <summary>
    /// Used to store player's data. Change it the way you want.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public Dictionary<ResType, int> PlayerInventory;
        public Dictionary<SlingType,Dictionary<UpType, int>> SlingUps;
        public SlingType CurrentSling;
        public List<SlingType> Slings;

    }
}