using UnityEngine;

namespace Logic
{
	public class GravityZone : MonoBehaviour
	{
		[SerializeField]
		private float _gravityScale;

		public static GravityZone Create()
		{
			return Instantiate(Resources.Load<GameObject>("Prefabs/GravityZone")).GetComponent<GravityZone>();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			Logic.Entity.Entity entity = other.GetComponent<Logic.Entity.Entity>();
			if (entity == null)
			{
				return;
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			Logic.Entity.Entity entity = other.GetComponent<Logic.Entity.Entity>();
			return;
		}
	}
}