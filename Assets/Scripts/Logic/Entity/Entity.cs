using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Entity
{
    public enum EntityType
    {
        Undefined = 0,
        Entity,
        Dust,
        Planet,
        Star,
        BlackHole,
    }

    public enum MoveType
    {
        Undefined = 0,
        Holded,     // 
        Linear,
        Curve,
        Cycle,
        Impacted
    }

    public class Entity : MonoBehaviour, IPoolable, Input.ITouchable
    {
        public virtual EntityType Type => EntityType.Entity;
        public override string ToString() => $"{Id}({Serial})";
        public Common.StaticData.EntityInfo Info { get; private set; }
        public bool IsPressed { get; private set; }
        public string Id => _id;
        private string _id;
        public int Serial => _serial;
        private int _serial;
        public int Level => level;
        public int level;

        public MoveType MoveState = MoveType.Undefined;
        public float impactedGravity;
        public float curveGravity;

        protected Coroutine dragLerpCoroutine;
        protected Vector3 dragPosDiff;
        protected Vector3 dragDestPos;
        protected Vector3 dragDir;

        public bool Invincible { get; private set; }

        public virtual float GetRadius()
        {
            return 0f;
        }

        public virtual void Init(string id, int serial)
        {
            _id = id;
            _serial = serial;
            level = 1;
            name = $"{Type}_{ToString()}";
            IsPressed = false;
            StopAllCoroutines();
            Info = Common.StaticInfo.StaticInfoManager.Instance.EntityInfos[_id];
            
            // Init StaticData
            moveSpeedBase = Info.Moves[level - 1].DefaultMovingSpeed;
            moveSpeedLevelRate = Info.Moves[level - 1].LevelSpeedRate;
            //

            // ������ ����
            angleRotate.x = 0f;
            angleRotate.y = 0f;
            angleRotate.z = UnityEngine.Random.value;

            affectedEntities.Clear();
            Invincible = true;
        }


        public virtual void OnChangeLevel()
        {

        }

        #region IPoolable
        public void OnInit()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnRelease()
        {
            gameObject.SetActive(false);
        }

        /// <summary> 행성, 항성, 블랙홀 충돌시 재생 </summary>
        public virtual void OnCollide()
        {

        }
        #endregion
        #region Input.ITouchable

        public virtual void OnPressDown()
        {
            IsPressed = true;
        }

        public virtual void OnPressUp()
        {
            IsPressed = false;
        }

        public virtual void OnStartDrag(Vector3 pos)
        {
            dragLerpCoroutine = StartCoroutine(DragLerpProcess());
            dragPosDiff = transform.position - pos;
        }

        private IEnumerator DragLerpProcess()
        {
            while (true)
            {
                transform.position = Vector3.Lerp(transform.position, dragDestPos, Time.deltaTime * 2f);
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
            moveDirection = dragDir.normalized;
            ChangeMoveState(null, MoveType.Linear);
        }

        #endregion


        public Vector3 GetAffectVector()
        {
            return Vector3.zero;
        }

        // ������Ʈ
        [Header("Component")]
        public SpriteRenderer _renderer;
        public TrailRenderer _trail;
        public float _trailTime = 0.5f;

        // �̵� �Ķ����
        [Header("Movement")]
        public Vector3 affectedVector;
        public Vector3 moveDirection;
        public Vector3 moveSpeedRate;     // �̵� ���ӵ�
        public float moveSpeedTotal;      // �̵��ӵ� - ���� �̵��ӵ�
        public float moveSpeedBase;       // �̵��ӵ� - �⺻
        public float moveSpeedLevelRate;  // �̵��ӵ� - �ܰ����
        protected delegate void Delegate_Move();
        protected Delegate_Move Move;

        public Dictionary<Entity, float> affectedEntities = new Dictionary<Entity, float>();

        // ���� �Ķ����
        [Header("Rotate")]
        public Vector3 angleRotate;
        public float rotateSpeed;

        // ������ �Ķ����
        [Header("Scale")]
        public float scaleBase;         // �⺻ ũ��
        public float scaleRate;         // �ܰ躰 ũ�� ����

        // ���� �Ķ����
        [Header("Settlate")]
        public float cycleNowRange = 1f;
        public float cycleNextRange = 0f;
        public float cyclePrevRange = 0f;
        public int cycleCount = 0;
        public int cycleCountMax = 5;
        public float cycleRotateSpeed = 72f;

        public virtual void ChangeMoveState(Entity hole, MoveType movetype) { }

        // ���� ������ �̵�
        protected readonly static string LayerMaskWall = "Wall";
        protected RaycastHit2D[] hit;
        public void WallOutReset(Transform hitWall)
        {
            hit = Physics2D.RaycastAll(transform.position, -moveDirection, 100f, 1 << LayerMask.NameToLayer(LayerMaskWall), -1, 1);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform != hitWall)
                {
                    transform.position = hit[i].point;
                }
            }
        }
    }
}