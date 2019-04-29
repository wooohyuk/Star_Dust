using UnityEngine;
using System.Collections.Generic;
using Logic.Entity;

namespace Input
{
	public class PressController : MonoBehaviour
	{
		[SerializeField]
		private InputController _inputController;

		private Logic.GravityZone _gravityZone;
		private DustGenerator _dustGenerator;

		private List<Logic.Entity.Entity> _entities = new List<Logic.Entity.Entity>();

		private void Awake()
		{
			_inputController.OnPressStart += OnPressStart;
			_inputController.OnPressUp += OnPressUp;
			_inputController.OnClick += OnClick;
			_inputController.OnTouchPosChange += OnTouchPosChange;
			_gravityZone = Logic.GravityZone.Create();
			_dustGenerator = GameObject.FindObjectOfType<DustGenerator>();
			_gravityZone.gameObject.SetActive(false);
		}

		private void OnClick(Vector3 pos)
		{
			pos = Camera.main.ScreenToWorldPoint(pos);
			pos.z = 0;

			RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.zero, 0f, ~(LayerMask.GetMask("Gravity")));
			for (int i = 0; i < hits.Length; ++i)
			{
				Entity entity = hits[i].transform?.GetComponent<Entity>();
				if (entity is Planet)
				{
					Planet planet = entity as Planet;
					EntityManager.Instance.Destroy(planet);
					_dustGenerator.CreateOnPlanetDestruction(planet.PlanetInfo.Growths[planet.Level - 1].DestroyDustCount, planet.transform.position);
					break;
				}
			}
			
		}

		private void OnTouchPosChange(Vector3 pos)
		{
			pos = Camera.main.ScreenToWorldPoint(pos);
			pos = new Vector3(pos.x, pos.y, 0f);
			_gravityZone.transform.position = pos;
		}

		private float _pressTime;
		private void OnPressStart(Vector3 pos)
		{
			_pressTime = Time.time;
			_entities.Clear();
			_gravityZone.gameObject.SetActive(true);
			pos = Camera.main.ScreenToWorldPoint(pos);

			RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.zero, 0f, ~(LayerMask.GetMask("Gravity")));
			for (int i = 0; i < hits.Length; ++i)
			{
				Entity entity = hits[i].transform?.GetComponent<Entity>();
				if (entity is Planet || entity is Star || entity is BlackHole)
				{
					_entities.Add(entity);
				}
			}
		}

		private void OnPressUp(Vector3 pos)
		{
			_gravityZone.gameObject.SetActive(false);
			for (int i = 0; i < _entities.Count; ++i)
			{
				_entities[i].OnPressUp();
			}

			if (_entities.Count == 0 && Time.time - _pressTime < 0.4f)
			{
				Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
				worldPos.z = 0;
				var createPlanet = new Logic.Situation.CreatePlanetSituation(worldPos, random: true);
			}
			_entities.Clear();
		}
	}
}