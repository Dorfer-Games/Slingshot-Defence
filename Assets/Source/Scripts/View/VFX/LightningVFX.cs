using System;
using UnityEngine;

namespace Source.Scripts.View.VFX
{
    public class LightningVFX : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] particleSystems;
        [SerializeField] private ParticleSystem[] particleSystemsHalfLenght;
        private ParticleSystemRenderer[] particleSystemsRenderers;
        private ParticleSystemRenderer[] particleSystemRenderersHalfLenght;

        private void Awake()
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                particleSystemsRenderers[i] = particleSystems[i].gameObject.GetComponent<ParticleSystemRenderer>();
            }

            for (int i = 0; i < particleSystemsHalfLenght.Length; i++)
            {
                particleSystemRenderersHalfLenght[i] =
                    particleSystemsHalfLenght[i].gameObject.GetComponent<ParticleSystemRenderer>();
            }
        }

        public void SetLenght(float lenght)
        {
            foreach (var systemRenderer in particleSystemsRenderers)
            {
                systemRenderer.lengthScale = lenght;
            }

            var half = lenght / 2f;
            foreach (var systemRenderer in particleSystemRenderersHalfLenght)
            {
                systemRenderer.lengthScale = half;
            }
        }
    }
}