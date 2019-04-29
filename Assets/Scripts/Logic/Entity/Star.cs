using Spine;
using Spine.Unity;
using UnityEngine;
using Common.StaticData;
using Common.StaticInfo;

namespace Logic.Entity
{
    /// <summary>
    /// 행성
    /// </summary>
    public class Star : Entity
    {
        public CircleCollider2D _col;
        private float _colOriginRadius;
        private CircleCollider2D _absorveCol;
        private float _absorveColOriginRadius;

        public int spriteIdx = 0;

        public override EntityType Type { get; } = EntityType.Star;

        public Common.StaticData.StarInfo StarInfo { get; private set; }
        private int _collectedDust = 0;
        private SkeletonAnimation _skeleton;

        public override float GetRadius()
        {
            return _absorveCol.radius * transform.localScale.x;
        }
        

        private void Awake()
        {
            _colOriginRadius = _col.radius;
            _skeleton = GetComponent<SkeletonAnimation>();
            SoundManager.Instance.Play("Sounds/Star_Born");
        }

        public override void Init(string id, int serial)
        {
            base.Init(id, serial);
            StarInfo = Common.StaticInfo.StaticInfoManager.Instance.EntityInfos[id] as Common.StaticData.StarInfo;
            OnChangeLevel();
            // B W Y
            if (id == "Star_1")
            {
                SoundManager.Instance.Play("Sounds/Planet_Destroy");
                _skeleton.Skeleton.SetSkin("B");
            }
            else if (id == "Star_2")
            {
                SoundManager.Instance.Play("Sounds/Planet_Destroy");
                _skeleton.Skeleton.SetSkin("W");
            }
            else if (id == "Star_3")
            {
                SoundManager.Instance.Play("Sounds/Planet_Destroy");
                _skeleton.Skeleton.SetSkin("Y");
            }

            _skeleton.state.SetAnimation(0, "create", false);
            _skeleton.AnimationState.Complete += OnCreateComplete;
            SoundManager.Instance.Play("Sounds/Star_Grow");
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

        public void AddAffectedEntity(Entity affectEntity, float gravityRate)
        {
            if (affectEntity.Type != EntityType.BlackHole)
                return;

            if (affectedEntities.ContainsKey(affectEntity))
                return;

            affectedEntities.Add(affectEntity, gravityRate);
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

        public override void OnCollide()
        {
            base.OnCollide();
            SoundManager.Instance.Play("Sounds/Star_Col");

        }

        private void OnCreateComplete(TrackEntry trackentry)
        {
            _skeleton.AnimationState.Complete -= OnCreateComplete;
            _skeleton.state.SetAnimation(0, "idle", true);
        }

        public override void OnRelease()
        {
            _skeleton.state.SetAnimation(0, "explosion", false);
            _skeleton.AnimationState.Complete += OnExplosionComplete;
            SoundManager.Instance.Play("Sounds/Star_Destroy");
        }

        private void OnExplosionComplete(TrackEntry trackentry)
        {
            _skeleton.AnimationState.Complete -= OnExplosionComplete;
            gameObject.SetActive(false);
        }


        public void SetAbsorve(StarAbsorve absorve)
        {
            _absorveCol = absorve.GetComponent<CircleCollider2D>();
            _absorveColOriginRadius = _absorveCol.radius;
        }

        public void CollectPlanet(Planet otherPlanet)
        {
            EntityManager.Instance.Destroy<Planet>(otherPlanet);
            ++_collectedDust;
            if (StarInfo.Growths.Count + 1 == level)
            {
            }
            else
            {
                if (_collectedDust >= StarInfo.Growths[level - 1].RequireStarDust)
                {
                    _collectedDust = 0;
                    ++level;
                    OnChangeLevel();
                }
            }
        }

        public void RemoveDust(Dust otherDust)
        {
            EntityManager.Instance.Destroy<Dust>(otherDust);
        }

        public override void OnChangeLevel()
        {
            base.OnChangeLevel();
            SoundManager.Instance.Play("Sounds/Star_Grow");
            if (level == StarInfo.Growths.Count + 1)
            {
                Vector3 pos = transform.position;
                EntityManager.Instance.Destroy(this);
                BlackHole nextBlackhole
                    = EntityManager.Instance.Create<BlackHole>(StaticInfoManager.Instance.EntityInfos["BlackHole"] as BlackHoleInfo);
                nextBlackhole.transform.position = pos;
                return;
            }
            float radiusScale = StarInfo.Growths[level - 1].Scale;
            this.transform.localScale = Vector3.one * radiusScale;
        }

        float impact = 30f, cycle = 120f;
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

                case EntityType.Planet:
                    Planet otherPlanet = (Planet)otherEntity;
                    Vector2 incomingVec = otherPlanet.moveDirection;
                    Vector2 normalVec = other.transform.position - transform.position;
                    Vector2 reflectVec = Vector2.Reflect(incomingVec, normalVec);
                    Vector2 angleVec = reflectVec - incomingVec;
                    float angle = Mathf.Atan2(reflectVec.y - incomingVec.y, reflectVec.x - incomingVec.x) * Mathf.Rad2Deg;
                    angle = Mathf.Abs(angle);
                    if (angle < impact) // 충돌
                    {
                        otherPlanet.ChangeMoveState(this, MoveType.Impacted);
                        //                        dust.ChangeMoveState(this, MoveType.Curve);
                    }
                    else if (angle < cycle) // 공전
                    {
                        otherPlanet.ChangeMoveState(this, MoveType.Cycle);
                        //dust.ChangeMoveState(this, MoveType.Curve);
                    }
                    else // 왜곡
                    {
                        otherPlanet.ChangeMoveState(this, MoveType.Curve);
                    }
                    break;

                default:
                    break;
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other == _absorveCol)
                return;

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
                    dust.RemoveAffectedEntity(this);
                    break;

                default:
                    break;
            }
        }
        
        public void CollectDust(Dust dust)
        {
            EntityManager.Instance.Destroy<Dust>(dust);
        }
    }
}