using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Source.Scripts.Data.Enum;

namespace Source.Scripts.Component
{
    public struct Tomes : IEcsAutoReset<Tomes>
    {
        public Dictionary<TomeType, int> Value;
        
        public void AutoReset(ref Tomes c)
        {
            c.Value = new Dictionary<TomeType, int>();
            var values = Enum.GetValues(typeof(TomeType));
                
            foreach (var item in values)
                c.Value.Add((TomeType)item,0);
        }
    }
}