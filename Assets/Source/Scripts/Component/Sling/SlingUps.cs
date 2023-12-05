using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Source.Scripts.Data.Enum;

namespace Source.Scripts.Component
{
    public struct SlingUps : IEcsAutoReset<SlingUps>
    {
        public Dictionary<SlingType,Dictionary<UpType, int>> Value;
        
        public void AutoReset(ref SlingUps c)
        {
            c.Value = new Dictionary<SlingType, Dictionary<UpType, int>>();
            var values = Enum.GetValues(typeof(SlingType));
            var upTypes = Enum.GetValues(typeof(UpType));

            foreach (var item in values)
            {
                var dictionary = new Dictionary<UpType, int>();

                foreach (var upType in upTypes)
                {
                    dictionary.Add((UpType)upType,0);
                }
                c.Value.Add((SlingType)item,dictionary);
            }
              
        }
    }
}