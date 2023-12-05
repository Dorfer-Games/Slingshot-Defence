using System;
using System.Collections;
using AYellowpaper.SerializedCollections;
using Cinemachine;
using DG.Tweening;
using Source.Scripts.Data.Enum;
using UnityEngine;

namespace Source.Scripts.View.Cam
{
    public class CameraSwitcherView : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<CameraPositionType, CinemachineVirtualCamera> cameras;
        private CameraPositionType currentType;
        private CinemachineBasicMultiChannelPerlin shake;
        private float shakeTime;
        private Coroutine shakeCoroutine;
        private bool blocked;

        private void Awake()
        {
            currentType = CameraPositionType.DOOR;
            shake = cameras[currentType].GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        }

        public void Switch(CameraPositionType type)
        {
            cameras[currentType].gameObject.SetActive(false);
            cameras[type].gameObject.SetActive(true);
            currentType = type;
        }

        public void ShowCamera(CameraPositionType type)
        {
            if (blocked)
                return;
            
            Switch(type);
            StartCoroutine(WaitSwitchToPlayer());
        }

        private IEnumerator WaitSwitchToPlayer()
        {
            yield return new WaitForSecondsRealtime(3f);
            Switch(CameraPositionType.DEFAULT);
        }

        public void ShowTarget(Transform target,float moveTime,float waitTime)
        {
            if (blocked)
                return;
            
            blocked = true;
            var cam = cameras[CameraPositionType.DEFAULT];
            var camFollow = cam.Follow;
            var camLookAt = cam.LookAt;
            var delta = target.position - camLookAt.position;
            cam.LookAt = null;
            cam.Follow = null;
            DOTween.Sequence()
                .SetUpdate(UpdateType.Normal,true)
                .Append(cam.transform.DOMove(delta, moveTime).SetRelative().SetEase(Ease.InOutSine))
                .AppendInterval(waitTime)
                .Append(cam.transform.DOMove(-delta, moveTime).SetRelative().SetEase(Ease.InOutSine))
                .OnComplete(() =>
                {
                    cam.LookAt = camLookAt;
                    cam.Follow = camFollow;
                    blocked = false;
                });
        }

        public void Shake(float value,float time)
        {
            shakeTime += time;
            shake.m_AmplitudeGain  += value;;
            if (shakeCoroutine==null)
            {
                shakeCoroutine=StartCoroutine(WaitShake());
            }
           
        }

        private IEnumerator WaitShake()
        {
            while (shakeTime>0)
            {
                shakeTime -= Time.deltaTime;
                yield return null;
            }

            shakeTime = 0;
            shake.m_AmplitudeGain = 0;
            shakeCoroutine = null;
        }
    }
}