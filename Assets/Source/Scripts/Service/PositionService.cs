using System;
using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Mono.Collections.Generic;
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


        public PositionService(EcsWorld world, SaveData save, GameData gameData, GameConfig config, Pools pool)
        {
            this.world = world;
            this.save = save;
            this.gameData = gameData;
            this.config = config;
            this.pool = pool;

            filterEnemy = world.Filter<Enemy>().End();
        }


        public List<int> GetEntInRadius(int startEnt, float radius, EcsFilter filter)
        {
            var list = new List<int>();
            var startPos = pool.View.Get(startEnt).Value.transform.position;
            startPos.y = 0;
            foreach (var ent in filter)
            {
                var position = pool.View.Get(ent).Value.transform.position;
                position.y = 0;
                if ((position - startPos).sqrMagnitude <= radius * radius)
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

        public List<int> GetEnemiesInRadiusWithPriority(int startEnt, float radius, ICollection<int> lowPriorList,
            bool strict)
        {
            var list = GetEnemiesInRadius(startEnt, radius);
            if (strict)
            {
                list.RemoveAll(lowPriorList.Contains);
                return list;
            }
            else
            {
                return GetPriorList(list,lowPriorList);
            }
        }

        private List<int> GetPriorList(List<int> high,ICollection<int> low)
        {
            var startList = new List<int>();
            var endList = new List<int>();
            foreach (var ent in high)
            {
                if (low.Contains(ent))
                    endList.Add(ent);
                else
                    startList.Add(ent);
            }

            startList.AddRange(endList);
            return startList;
        }


        public List<int> GetClosestSequence(int startEnt, float radius, int count, ICollection<int> lowPriorList)
        {  
            var sortedList = new List<int>();
            sortedList.Add(startEnt);
            int startE = startEnt;
            var lowUniq = new List<int>();
            lowUniq.AddRange(lowPriorList);
            
            for (int i = 0; i < count-1; i++)
            {
                var list = GetEnemiesInRadiusWithPriority(startE, radius,lowUniq,true);
                if (list.Count == 0)
                    list.AddRange(lowUniq);

                list.Remove(startE);
                if (list.Count==0)
                {
                    break;
                }
                var closest = GetClosest(startE,list);
                if (!lowUniq.Contains(closest))
                    lowUniq.Add(closest);
                startE = closest;
                sortedList.Add(closest);
            }

            return sortedList;
        }


        public int GetClosest(int startEnt,List<int> list)
        {
            var startPos = pool.View.Get(startEnt).Value.transform.position;
            int closestE = -1;
            float closestSqrMag = Single.PositiveInfinity;
            foreach (var ent in list)
            {
                var position = pool.View.Get(ent).Value.transform.position;
                position.y = 0;
                float sqrMag = (position-startPos).sqrMagnitude;
                if (sqrMag<closestSqrMag)
                {
                    closestE = ent;
                    closestSqrMag = sqrMag;
                }
            }

            return closestE;
        }
    }
}