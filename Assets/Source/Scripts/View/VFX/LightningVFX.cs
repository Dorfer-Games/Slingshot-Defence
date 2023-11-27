using System;
using UnityEngine;

namespace Source.Scripts.View.VFX
{
    public class LightningVFX : HitVFXBase
    {
        [SerializeField] private ParticleSystem[] particleSystems;
        [SerializeField] private ParticleSystem[] particleSystemsHalfLenght;
        private ParticleSystemRenderer[] particleSystemsRenderers;
        private ParticleSystemRenderer[] particleSystemRenderersHalfLenght;

        public override void Init()
        {
            particleSystemsRenderers = new ParticleSystemRenderer[particleSystems.Length];
            particleSystemRenderersHalfLenght = new ParticleSystemRenderer[particleSystemsHalfLenght.Length];
            for (int i = 0; i < particleSystems.Length; i++)
            {
                particleSystemsRenderers[i] = particleSystems[i].GetComponent<ParticleSystemRenderer>();
            }

            for (int i = 0; i < particleSystemsHalfLenght.Length; i++)
            {
                particleSystemRenderersHalfLenght[i] =
                    particleSystemsHalfLenght[i].GetComponent<ParticleSystemRenderer>();
            }
        }

        public void SetLength(float lenght)
        {
            lenght *= 2;
            foreach (var systemRenderer in particleSystemsRenderers)
            {
                systemRenderer.lengthScale = lenght;
            }

            var half = lenght * 1f;
            foreach (var systemRenderer in particleSystemRenderersHalfLenght)
            {
                systemRenderer.lengthScale = half;
            }
        }
    }
}