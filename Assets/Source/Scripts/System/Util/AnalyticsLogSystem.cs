using Ketchapp.MayoSDK;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.Event;

namespace Source.Scripts.System.Util
{
    public class AnalyticsLogSystem : GameSystem
    {
        private EcsFilter filterLogEvent;
       
        public override void OnInit()
        {
            base.OnInit();
            filterLogEvent = eventWorld.Filter<AnalyticsEvent>().End();
            
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filterLogEvent)
            {
                KetchappSDK.Analytics.CustomEvent(pool.AnalyticsEvent.Get(e).Value);
            }
        }
    }
}