﻿using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Scripts.View
{
    public class AnimationView : MonoBehaviour
    {
        [SerializeField] private GameObject visual;
        [SerializeField] private ParticleSystem hitVFX;
        [SerializeField] private float minBounce;
        [SerializeField] private float maxBounce;

        public float DeathAnimLenght;

        private Animator visualAnimator;
        private Collider[] colliders;
        
        private bool BlockAnim=false;
        private Coroutine attackCoroutine;

        private void Awake()
        {
            visualAnimator = visual.GetComponentInChildren<Animator>();
            colliders = GetComponentsInChildren<Collider>();
        }

        public void AnimateDeath()
        {
            visualAnimator.Play("Die");
            BlockAnim = true;
            foreach (var col in colliders)
                col.enabled = false;
        }

        public void Play(string name)
        {
            if (!gameObject.activeSelf || BlockAnim)
                return;
            
            visualAnimator.Play(name);
        }

        public void ToggleAttack(bool isAttack,float speed=1f)
        {
            visualAnimator.speed = speed;
            if (attackCoroutine==null && !isAttack && visualAnimator.GetLayerWeight(1)>=1)
            {
                attackCoroutine=StartCoroutine(WaitAtk());
            }else
            if (isAttack)
            {
                visualAnimator.SetLayerWeight(1,1);
            }
           
        }

        private IEnumerator WaitAtk()
        {
            for (float i = 0; i < 1f; i+=Time.deltaTime/0.2f)
            {
                visualAnimator.SetLayerWeight(1,1-i);
                yield return null;
            }
            visualAnimator.SetLayerWeight(1,0);
            attackCoroutine = null;
        }

        public void Bounce()
        {
            var range = Random.Range(0, 2);
            var bounce = range == 0 ? minBounce : maxBounce;
            DOTween.Sequence()
                .SetLink(gameObject)
                .Append(visual.transform.DOScale(bounce, 0.15f))
                .Append(visual.transform.DOScale(1f, 0.15f));
        }

        public void GetHitVFX()
        {
            hitVFX.Stop();
            hitVFX.gameObject.SetActive(true);
            hitVFX.Play();
        }
    }
}