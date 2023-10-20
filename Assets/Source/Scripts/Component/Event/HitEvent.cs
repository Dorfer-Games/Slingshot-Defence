using System.Collections.Generic;

namespace Source.Scripts.Component.Event
{
    public struct HitEvent
    {
        public int Sender;
        public List<int> Targets;
    }
}