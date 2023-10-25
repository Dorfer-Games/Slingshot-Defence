using System;
using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component;
using Source.Scripts.Component.ViewComponent;
using Source.Scripts.System.Util;
using Source.Scripts.View;
using UnityEngine;

namespace Source.Scripts.Service
{
    public class PositionService
    {
        private EcsWorld world;
        private readonly SaveData save;
        private readonly GameData gameData;
        private readonly GameConfig config;
        private readonly Pools pool;
        
        private readonly EcsFilter filterEnemy;


        public PositionService(EcsWorld world, SaveData save, GameData gameData, GameConfig config,Pools pool)
        {
            this.world = world;
            this.save = save;
            this.gameData = gameData;
            this.config = config;
            this.pool = pool;
            
            filterEnemy = world.Filter<Enemy>().End();
        }

        public List<int> GetEntInRadius(int startEnt,float radius,EcsFilter filter)
        {
            var list = new List<int>();
            var startPos = pool.View.Get(startEnt).Value.transform.position;
            startPos.y = 0;
            foreach (var ent in filter)
            {
                var position = pool.View.Get(ent).Value.transform.position;
                position.y = 0;
               
                if ((position-startPos).magnitude<= radius)
                {
                    list.Add(ent);
                }
            }

            return list;
        }

        public List<int> GetEnemiesInRadius(int startEnt, float radius)
        {
            return GetEntInRadius(startEnt, radius, filterEnemy);
        }

        public List<int> GetEnemiesInRadiusWithPriority(int startEnt, float radius,List<int> lowPriorList,bool strict)
        {
            var list = GetEnemiesInRadius(startEnt, radius);
            if (strict)
            {
                list.RemoveAll(lowPriorList.Contains);
                return list;
            }
            else
            {
                var startList = new List<int>();
                var endList = new List<int>();
                foreach (var ent in list)
                {
                    if (lowPriorList.Contains(ent))
                        endList.Add(ent);
                    else
                        startList.Add(ent);
                }
                startList.AddRange(endList);
                return startList;
            }
            
        }
    }
}