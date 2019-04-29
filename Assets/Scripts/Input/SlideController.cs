using UnityEngine;
using System.Collections.Generic;

namespace Input
{
	public class SlideController : MonoBehaviour
	{
		[SerializeField]
		private InputController _inputController;

		private List<Logic.Entity.Entity> _entities = new List<Logic.Entity.Entity>();
		private Logic.Entity.Entity _mainEntity = null;
		
		private void Awake()
		{
			_inputController.OnSlide += OnSlide;
			_inputController.OnTouchUp += OnTouchUp;
			_inputController.OnPressUp += InputControllerOnOnPressUp;
		}

		private void InputControllerOnOnPressUp(Vector3 obj)
		{
			_entities.ForEach(e => e.OnEndDrag());
			_entities.Clear();
			_mainEntity = null;
		}

		private void OnSlide(Vector3 startPoint, Vector3 endPoint)
		{
			startPoint = Camera.main.ScreenToWorldPoint(startPoint);
			endPoint = Camera.main.ScreenToWorldPoint(endPoint);
			startPoint = new Vector3(startPoint.x, startPoint.y, 0f);
			endPoint = new Vector3(endPoint.x, endPoint.y, 0f);
			Vector3 delta = endPoint - startPoint;
			
			Collider2D[] hits = Physics2D.OverlapCircleAll(endPoint, 1f);
			for (int i = 0; i < hits.Length; ++i)
			{
				Logic.Entity.Entity entity = hits[i].transform?.GetComponent<Logic.Entity.Entity>();
				if (entity == null)
				{
					continue;
				}

				if (entity.GetRadius() + 1f < (endPoint - entity.transform.position).magnitude)
				{
					continue;
				}

				
				if ((entity is Logic.Entity.Dust) == false)
				{
					if (_mainEntity != null)
					{
						continue;
					}

					_mainEntity = entity;
				}

				if (_entities.Contains(entity) == false)
				{
					entity.OnStartDrag(endPoint);
					_entities.Add(entity);
				}
			}

			for (int i = 0; i < _entities.Count; ++i)
			{
				_entities[i].OnDrag(endPoint, delta.normalized);
			}
		}

		

		private void OnTouchUp(Vector3 pos)
		{
//			_entities.ForEach(e => e.OnEndDrag());
//			_entities.Clear();
		}
	}
}