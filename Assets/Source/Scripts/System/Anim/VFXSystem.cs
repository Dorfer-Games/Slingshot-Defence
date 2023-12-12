using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Component.Event;

namespace Source.Scripts.System.Anim
{
    public class VFXSystem : GameSystem
    {
        private EcsFilter filterEvent;
        public override void OnInit()
        {
            base.OnInit();
            filterEvent = eventWorld.Filter<VFXEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filterEvent)
            {
                var vfxEvent = pool.VFXEvent.Get(e);
                var vfxId = vfxEvent.VFXId;
                var target = vfxEvent.Target;
                var toggle = vfxEvent.Toggle;

                if (pool.HitVFXProviderComponent.Has(target))
                {
                    var vfxProviderView = pool.HitVFXProviderComponent.Get(target).Value;
                    vfxProviderView.VFXs[vfxId].gameObject.SetActive(toggle);
                }
            }
        }
    }
}