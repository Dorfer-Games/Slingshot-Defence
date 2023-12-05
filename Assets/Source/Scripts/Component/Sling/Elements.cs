using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Source.Scripts.Data.Enum;

namespace Source.Scripts.Component
{
    public struct Elements : IEcsAutoReset<Elements>
    {
        public Dictionary<ElementType, int> Value;
        
        public void AutoReset(ref Elements c)
        {
            c.Value = new Dictionary<ElementType, int>();
            var values = Enum.GetValues(typeof(TomeType));
                
            foreach (var item in values)
                c.Value.Add((ElementType)item,0);
        }
    }
}