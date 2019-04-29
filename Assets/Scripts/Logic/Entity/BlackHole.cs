using System.Collections;
using UnityEngine;
using Spine;
using Spine.Unity;

namespace Logic.Entity
{
	/// <summary>
	/// 행성
	/// </summary>
	public class BlackHole : Entity
	{
		public override EntityType Type => EntityType.BlackHole;

		public Common.StaticData.BlackHoleInfo BlackHoleInfo { get; private set; }

		[SerializeField]
		private float _aliveTime;
		private SkeletonAnimation _skeleton;

		private void Awake()
		{
			_skeleton = GetComponent<SkeletonAnimation>();
		}

		public override void Init(string id, int serial)
		{
			base.Init(id, serial);
			BlackHoleInfo = Common.StaticInfo.StaticInfoManager.Instance.EntityInfos[id] as Common.StaticData.BlackHoleInfo;
			_skeleton.state.SetAnimation(0, "create", false);
			_skeleton.AnimationState.Complete += OnCreateComplete;
		}

		private IEnumerator WaitForDestroy()
		{
			yield return new WaitForSeconds(_aliveTime);
            SoundManager.Instance.Play("Sounds/BH_Destroy_2");
            EntityManager.Instance.Destroy(this);
		}

		private void OnCreateComplete(TrackEntry trackentry)
		{
			_skeleton.AnimationState.Complete -= OnCreateComplete;
			_skeleton.state.SetAnimation(0, "idle", true);
		}

		public override void OnRelease()
		{
			_skeleton.state.SetAnimation(0, "remove", false);
			_skeleton.AnimationState.Complete += OnRemoveComplete;
		}

		private void OnRemoveComplete(TrackEntry trackentry)
		{
			_skeleton.AnimationState.Complete -= OnRemoveComplete;
			gameObject.SetActive(false);
		}

		public void OnTriggerEnter2D(Collider2D other)
        {
            Entity otherEntity = other.GetComponent<Entity>();
            if (otherEntity == null)
            {
                return;
            }

	        if (otherEntity.Type == EntityType.Dust)
	        {
		        EntityManager.Instance.Destroy(otherEntity as Dust);
	        }
	        else if (otherEntity.Type == EntityType.Planet)
	        {
		        EntityManager.Instance.Destroy(otherEntity as Planet);
	        }
	        else if (otherEntity.Type == EntityType.Star)
	        {
		        EntityManager.Instance.Destroy(otherEntity as Star);
	        }
	        else if (otherEntity.Type == EntityType.BlackHole)
	        {
				EntityManager.Instance.Destroy(otherEntity as BlackHole);
	        }
        }
	}
}