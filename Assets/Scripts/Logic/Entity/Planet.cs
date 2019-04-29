using Common.StaticData;
using Common.StaticInfo;
using UnityEngine;
using System.Collections.Generic;

namespace Logic.Entity
{
    /// <summary>
    /// 행성
    /// </summary>
    public enum Element
    {
        Normal = 0,
        Fire,
        Ice,
        Iron,
        Gas,
        Tree
    }
    public class Planet : Entity
    {
        public Sprite[] _sprites;

        public CircleCollider2D _col;
        private float _colOriginRadius;
        private CircleCollider2D _absorveCol;
        private float _absorveColOriginRadius;

        private float _originCycleNowRange;
        
        public Star cycleCore = null;
        public Element elementId = 0;

        public List<Dust> satellites = new List<Dust>();

        public override EntityType Type => EntityType.Planet;

        public Common.StaticData.PlanetInfo PlanetInfo { get; private set; }
        private int _collectedDust = 0;

        public override float GetRadius()
        {
            return _absorveCol.radius;
        }

        private void Awake()
        {
            _colOriginRadius = _col.radius;
            _originCycleNowRange = cycleNowRange;

            SoundManager.Instance.Play("Sounds/Planet_Born");
        }

        public override void Init(string id, int serial)
        {
            base.Init(id, serial);
            PlanetInfo = Common.StaticInfo.StaticInfoManager.Instance.EntityInfos[id] as Common.StaticData.PlanetInfo;
            OnChangeLevel();

            // 초기 이동상태
            Move = null;

            cycleCount = 0;
        }

        public override void OnRelease()
        {
            base.OnRelease();
            SoundManager.Instance.Play("Sounds/Planet_Destroy");
        }

        public override void OnCollide()
        {
            base.OnCollide();
            SoundManager.Instance.Play("Sounds/Planet_Col");
        }

        private void Update()
        {
            transform.Rotate(angleRotate * rotateSpeed * Time.deltaTime);
            Move?.Invoke();
        }

        public void SetAbsorve(PlanetAbsorve absorve)
        {
            _absorveCol = absorve.GetComponent<CircleCollider2D>();
            _absorveColOriginRadius = _absorveCol.radius;
        }

        public void CollectDust(Dust dust)
        {
            EntityManager.Instance.Destroy<Dust>(dust);
            ++_collectedDust;
            if (_collectedDust >= PlanetInfo.Growths[level - 1].RequireStarDust)
            {
                _collectedDust = 0;
                ++level;
                OnChangeLevel();
            }
        }

        public override void OnChangeLevel()
        {
            base.OnChangeLevel();
            SoundManager.Instance.Play("Sounds/Planet_Grow");
            if (level == PlanetInfo.Growths.Count + 1)
            {
                Vector3 pos = transform.position;
                EntityManager.Instance.Destroy(this);
                Star star = EntityManager.Instance.Create<Star>(StaticInfoManager.Instance.EntityInfos["Star_" + Random.Range(1, 4).ToString()] as StarInfo);
                star.transform.position = pos;
                return;
            }
            _renderer.sprite = _sprites[level - 1];
            float radiusScale = PlanetInfo.Growths[level - 1].Scale;
            cycleNowRange = _originCycleNowRange * radiusScale;
            _col.radius = _colOriginRadius * radiusScale;
            _absorveCol.radius = _absorveColOriginRadius * radiusScale;
        }

        // 이동
        public override void ChangeMoveState(Entity hole, MoveType movetype)
        {
            if (_trail != null)
            {
                _trail.enabled = false;
            }
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
                    cycleCore = (Star)hole;
                    if (cycleCore.cycleCount >= cycleCore.cycleCountMax)
                    {
                        cycleCore = null;
                        ChangeMoveState(null, MoveType.Linear);
                        return;
                    }
                    else
                    {
                        affectedEntities.Clear();
                        cycleCore = (Star)hole;
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
            if (_trail != null)
            {
                _trail.enabled = true;
            }
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

        // 이동에 영향을 주는 행성과 중력값 리스트 등록
        public void AddAffectedEntity(Entity affectEntity, float gravityRate)
        {
            if (affectEntity.Type == EntityType.Dust
                || affectEntity.Type == EntityType.Planet)
                return;

            if (affectedEntities.ContainsKey(affectEntity))
                return;

            affectedEntities.Add(affectEntity, gravityRate);
        }

        // 충돌처리
        float impact = 45f, cycle = 90f;
        public void OnTriggerEnter2D(Collider2D other)
        {
            Entity otherEntity = other.GetComponent<Entity>();
            if (otherEntity == null)
            {
                return;
            }

            // 상위개체와 충돌함 (Star, Blackhole)
            if (otherEntity.Type > Type)
                return;

            // 동일개체와 충돌함 (Planet)

            // 하위개체와 추돌함 (Dust)
            switch (otherEntity.Type)
            {
                case EntityType.Undefined:
                    break;

                case EntityType.Dust:
                    Dust dust = (Dust)otherEntity;
                    Vector2 incomingVec = dust.moveDirection;
                    Vector2 normalVec = other.transform.position - transform.position;
                    Vector2 reflectVec = Vector2.Reflect(incomingVec, normalVec);
                    Vector2 angleVec = reflectVec - incomingVec;
                    float angle = Mathf.Atan2(reflectVec.y - incomingVec.y, reflectVec.x - incomingVec.x) * Mathf.Rad2Deg;
                    angle = Mathf.Abs(angle);
                    if (angle < impact) // 충돌
                    {
                        dust.ChangeMoveState(this, MoveType.Impacted);
                        //                        dust.ChangeMoveState(this, MoveType.Curve);
                    }
                    else if (angle < cycle) // 공전
                    {
                        dust.ChangeMoveState(this, MoveType.Cycle);
                        //dust.ChangeMoveState(this, MoveType.Curve);
                    }
                    else // 왜곡
                    {
                        dust.ChangeMoveState(this, MoveType.Curve);
                    }
                    break;

                default:
                    break;
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            Entity otherEntity = other.GetComponent<Entity>();
            if (otherEntity == null)
            {
                return;
            }

            // 상위개체와 충돌함 (Star, Blackhole)
            if (otherEntity.Type >= Type)
                return;

            // 동일개체와 충돌함 (Planet)

            // 하위개체와 추돌함 (Dust)
            switch (otherEntity.Type)
            {
                case EntityType.Undefined:
                    break;

                case EntityType.Dust:
                    Dust dust = (Dust)otherEntity;
                    dust.RemoveAffectedEntity(this);
                    break;

                default:
                    break;
            }
        }


    }
}