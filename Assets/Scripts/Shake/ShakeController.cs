using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Shake
{
    public class ShakeController : MonoBehaviour
    {
        public bool ShakePosition { get; set; }
        public bool ShakeRotation { get; set; }

        public List<ShakeInstance> _shakeInstances = new List<ShakeInstance>();

        private Vector3 _originPosition;
        private Vector3 _originRotation;
        private float _freezeSpeed = 1f;

        private Transform _target;

        public void SetTarget(Transform target, Vector3 initialLocalPos, Vector3 initialLocalRotation)
        {
            _target = target;
            OriginPositionChanged(initialLocalPos);
            OriginRotationChanged(initialLocalRotation);
        }

        public void OriginPositionChanged(Vector3 position)
        {
            _originPosition = position;
        }

        public void OriginRotationChanged(Vector3 rotation)
        {
            _originRotation = rotation;
        }

        public void SetFreezeSpeed(float freezeSpeed)
        {
            this._freezeSpeed = freezeSpeed;
            for(int i = 0; i < _shakeInstances.Count; ++i)
            {
                _shakeInstances[i].ScaleRoughness = this._freezeSpeed;
            }
        }

        public void StopAll()
        {
            _shakeInstances.Clear();
            if (_target == null)
            {
                return;
            }

            _target.localPosition = _originPosition;
            _target.localEulerAngles = _originRotation;
        }

        private void Update()
        {
            if(_target == null)
            {
                return;
            }

            Vector3 posAddShake = _originPosition;
            Vector3 rotAddShake = _originRotation;

            for(int i = 0; i < _shakeInstances.Count; i++)
            {
                if(i >= _shakeInstances.Count)
                    break;

                ShakeInstance c = _shakeInstances[i];

                if(c.CurrentState == ShakeState.Inactive && c.DeleteOnInactive)
                {
                    _shakeInstances.RemoveAt(i);
                    --i;
                }
                else if(c.CurrentState != ShakeState.Inactive)
                {
                    posAddShake += MultiplyVectors(c.UpdateShake(), c.PositionInfluence);
                    rotAddShake += MultiplyVectors(c.UpdateShake(), c.RotationInfluence);
                }
            }

            if(ShakePosition == true)
            {
                _target.localPosition = posAddShake;
            }
            if(ShakeRotation == true)
            {
                _target.localEulerAngles = rotAddShake;
            }
        }

        public ShakeHolder StartShake(string shakeInfoId)
        {
            if(_target == null)
            {
                return null;
            }
            if(shakeInfoId == null || Common.StaticInfo.StaticInfoManager.Instance.ShakeInfos.Exist(shakeInfoId) == false)
            {
                return null;
            }

            Common.StaticData.ShakeInfo shakeInfo = Common.StaticInfo.StaticInfoManager.Instance.ShakeInfos[shakeInfoId];
            return StartShake(shakeInfo);
        }

        public ShakeHolder StartShake(Common.StaticData.ShakeInfo shakeInfo)
        {
            if(_target == null || shakeInfo == null)
            {
                return null;
            }

            ShakeInstance newInstance = new ShakeInstance(shakeInfo.FadeInTime, shakeInfo.FadeOutTime)
            {
                Magnitude = shakeInfo.Scale,
                Roughness = shakeInfo.Speed,
                PositionInfluence = shakeInfo.PositionInfluence.Convert(),
                RotationInfluence = shakeInfo.RotationInfluence.Convert()
            };

            newInstance.ScaleRoughness = this._freezeSpeed;

            StartCoroutine(
                        ShakeRoutine(newInstance, shakeInfo));

            return new ShakeHolder(newInstance, shakeInfo.FadeOutTime);
        }

        private IEnumerator ShakeRoutine(ShakeInstance shakeInstance, Common.StaticData.ShakeInfo shakeInfo)
        {
            if (shakeInfo.Delay > 0)
            {
                yield return new WaitForSeconds(shakeInfo.Delay);
            }
            shakeInstance.StartFadeIn(shakeInfo.FadeInTime);
            _shakeInstances.Add(shakeInstance);

            if(shakeInfo.Duration <= 0f) // 무한 쉐이크, StartShake()로 리턴받은 ShakeInstance를 가지고 수동으로 꺼줘야 한다. ShakeHolder 참고
            {
                yield break;
            }

            float elapsedTime = 0f;
            float endTime = shakeInfo.Duration + shakeInfo.FadeInTime;
            while(elapsedTime <= endTime)
            {
                elapsedTime += Time.deltaTime * _freezeSpeed;
                yield return null;
            }

            if(shakeInstance.CurrentState != ShakeState.FadingOut)
            {
                shakeInstance.StartFadeOut(shakeInfo.FadeOutTime);
            }
        }

        private static Vector3 MultiplyVectors(Vector3 v, Vector3 w)
        {
            v.x *= w.x;
            v.y *= w.y;
            v.z *= w.z;

            return v;
        }
    }
}