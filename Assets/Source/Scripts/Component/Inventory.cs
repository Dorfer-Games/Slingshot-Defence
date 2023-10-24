using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Source.Scripts.Data.Enum;

namespace Source.Scripts.Component
{
    public struct Inventory : IEcsAutoReset<Inventory>
    {
        public Dictionary<ResType, int> Value;

        public void AutoReset(ref Inventory c)
        {
            c.Value = new Dictionary<ResType, int>();
            var values = Enum.GetValues(typeof(ResType));
                
            foreach (var item in values)
                c.Value.Add((ResType)item,0);
        }
    }
}