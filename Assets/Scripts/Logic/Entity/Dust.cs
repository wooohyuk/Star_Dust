using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Logic.Entity
{
    /// <summary>
    /// 우주 먼지
    /// </summary>
    public class Dust : Entity
    {
        public override EntityType Type => EntityType.Dust;

        public Sprite[] _sprites = null;

        public Planet cycleCore = null;

        private CircleCollider2D _dustCol;

        private void Awake()
        {
            _dustCol = GetComponent<CircleCollider2D>();
        }

        public void Start()
        {
            if (Testment.testment.isTest)
            {
                moveDirection.x = Mathf.Round(UnityEngine.Random.Range(-1f, 1f) * 100f) / 100f;
                moveDirection.y = Mathf.Round(UnityEngine.Random.Range(-1f, 1f) * 100f) / 100f;
                moveDirection.z = 0f;
                moveDirection = moveDirection.normalized;
            }
        }

        public override float GetRadius()
        {
            return _dustCol.radius * transform.localScale.x;
        }

        public override void Init(string id, int serial)
        {
            base.Init(id, serial);
            DustInfo = Common.StaticInfo.StaticInfoManager.Instance.EntityInfos[id] as Common.StaticData.DustInfo;

            ChangeMoveState(null, MoveType.Linear);
            transform.SetParent(null);
            _renderer.sprite = _sprites[0];
            _trail.time = 0f;
            _trail.enabled = false;
        }

        public void Update()
        {
            if (Testment.testment != null && Testment.testment.isTest == true)
            {
                rotateSpeed = Testment.testment.dust_rotateSpeed;
                moveSpeedBase = Testment.testment.dust_moveSpeedBase;
                moveSpeedLevelRate = 1f;

                scaleBase = Testment.testment.dust_scaleBase;
                scaleRate = 1f;
            }

            transform.Rotate(angleRotate * rotateSpeed * Time.deltaTime);
            Move?.Invoke();
        }

        public override void ChangeMoveState(Entity hole, MoveType movetype)
        {
            _trail.enabled = false;
            switch (movetype)
            {
                case MoveType.Undefined:
                    break;

                case MoveType.Holded:
                    AddAffectedEntity(hole, hole.impactedGravity);
                    Move = null;
                    break;

                case MoveType.Linear:
                    affectedEntities.Clear();
                    Move = MoveLinear;
                    break;

                case MoveType.Curve:
                    AddAffectedEntity(hole, hole.curveGravity);
                    Move = MoveLinear;
                    break;

                case MoveType.Cycle:
                    cycleCore = (Planet)hole;
                    if (cycleCore.cycleCount >= cycleCore.cycleCountMax)
                    {
                        cycleCore = null;
                        ChangeMoveState(null, MoveType.Linear);
                        return;
                    }
                    else
                    {
                        affectedEntities.Clear();
                        cycleCore = (Planet)hole;
                        Move = MoveCycleStart;
                    }
                    break;

                case MoveType.Impacted:
                    AddAffectedEntity(hole, hole.impactedGravity);
                    Move = MoveLinear;
                    break;

                default:
                    break;
            }

            MoveState = movetype;
        }

        public void MoveCycleStart()
        {
            MoveLinear();
            if (cycleCore.cycleCount >= cycleCore.cycleCountMax)
            {
                ChangeMoveState(null, MoveType.Linear);
                return;
            }

            float nowDist = Vector2.Distance(cycleCore.transform.position, transform.position);
            if (nowDist > cycleCore.cycleNowRange)
                return;

            //transform.position = cycleCore.transform.position - transform.position;
            cycleCore.cycleCount++;
            _renderer.sprite = _sprites[(int)cycleCore.elementId];
            _trail.time = _trailTime;
            _trail.enabled = true;
            cycleCore.satellites.Add(this);
            transform.SetParent(cycleCore.transform);
            Move = MoveCycleLoop;
        }
        
        public void MoveCycleLoop()
        {
            if (cycleCore == null)
                ChangeMoveState(null, MoveType.Linear);
            
            transform.RotateAround(cycleCore.transform.position, Vector3.forward, cycleRotateSpeed * Time.deltaTime);
            transform.position = cycleCore.transform.position + (transform.position - cycleCore.transform.position).normalized * cycleCore.cycleNowRange;
        }

        public void MoveLinear()
        {
            moveSpeedTotal = moveSpeedBase * moveSpeedLevelRate;

            affectedVector = Vector3.zero;

            if (affectedEntities != null)
                foreach (var hole in affectedEntities)
                {
                    affectedVector += (hole.Key.transform.position - transform.position).normalized
                        * moveSpeedTotal * hole.Value * Time.deltaTime;
                }

            moveDirection = (moveDirection * moveSpeedTotal * Time.deltaTime + affectedVector).normalized;

            transform.position += moveDirection * moveSpeedTotal * Time.deltaTime;
        }

        public void AddAffectedEntity(Entity affectEntity, float gravityRate)
        {
            if (affectEntity.Type == EntityType.Dust)
                return;

            if (affectedEntities.ContainsKey(affectEntity))
                return;

            affectedEntities.Add(affectEntity, gravityRate);
        }

        public void RemoveAffectedEntity(Entity affectEntity)
        {
            if (affectEntity.Type == EntityType.Dust)
                return;

            affectedEntities.Remove(affectEntity);
        }

        public Common.StaticData.DustInfo DustInfo { get; private set; }


        public void SetParameter(Vector2 pos, Vector2 dir)
        {
            transform.position = pos;
            moveDirection = dir.normalized;
        }
        
        public virtual void OnStartDrag(Vector3 pos)
        {
//            ChangeMoveState(null, MoveType.Undefined);
            dragLerpCoroutine = StartCoroutine(DragLerpProcess());
            dragPosDiff = transform.position - pos;
        }

        private IEnumerator DragLerpProcess()
        {
            while (true)
            {
                transform.position = Vector3.Lerp(transform.position, dragDestPos, Time.deltaTime);
                yield return null;
            }
        }

        public virtual void OnDrag(Vector3 pos, Vector3 dir)
        {
            dragDestPos = pos;
            dragDir = dir;
        }

        public virtual void OnEndDrag()
        {
            StopCoroutine(dragLerpCoroutine);
        }
    }
}