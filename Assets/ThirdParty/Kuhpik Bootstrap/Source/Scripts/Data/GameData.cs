using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Leopotam.EcsLite;
using Source.Scripts.Service;
using Source.Scripts.System;
using Source.Scripts.System.Util;
using Source.Scripts.View;
using Source.Scripts.View.Cam;

namespace Kuhpik
{
    /// <summary>
    /// Used to store game data. Change it the way you want.
    /// </summary>
    [Serializable]
    public class GameData
    {
        public EcsWorld World = new EcsWorld();
        public EcsWorld EventWorld = new EcsWorld();
        public Fabric Fabric;
        public PositionService PositionService;
        public Pools Pools;
        public int PlayerEntity;
        public int StageEntity;
        
        public PlayerView PlayerView;
        public CameraSwitcherView CameraSwitcherView;
        public Joystick Joystick;
        
        //game
        public int LvlId;
        public int CurExp;
        public int KillsCount;
        public bool IsFinished;
    }
}