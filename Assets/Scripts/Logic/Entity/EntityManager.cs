using System.Collections.Generic;

namespace Logic.Entity
{
	public class EntityManager : Utility.SingletonMonoBehaviour<EntityManager>
	{
        public int _entityBestCount = 0, _entityMaxCount = 500;
		private Dictionary<int /*Serial*/, Entity> _entities = new Dictionary<int, Entity>();
		private Dictionary<EntityType, List<Entity>> _entityTypeLists = new Dictionary<EntityType, List<Entity>>();
		private int _currentSerial;
		public override void Init()
		{
		}

		public T Create<T>(Common.StaticData.EntityInfo entityInfo) where T : Entity
		{
			T entity = PoolManager<T>.Instance.Get(entityInfo.Id);
			int serial = ++_currentSerial;
			entity.Init(entityInfo.Id, serial);
			_entities.Add(serial, entity);
			if (_entityTypeLists.ContainsKey(entity.Type) == false)
			{
				_entityTypeLists.Add(entity.Type, new List<Entity>());
			}
			_entityTypeLists[entity.Type].Add(entity);

            if (_entityBestCount == _entityMaxCount)
                return entity;

            if (_entityBestCount < _entities.Count)
            {
                _entityBestCount = _entities.Count;
                CameraManager.Instance.SmoothCameraSizeUp((float)_entityBestCount / (float)_entityMaxCount);
            }

			return entity;
		}

		public void Destroy<T>(T entity) where T : Entity
		{
			PoolManager<T>.Instance.Release(entity, entity.Id);
			_entities.Remove(entity.Serial);
			_entityTypeLists[entity.Type].Remove(entity);
		}

		public void DestroyAll()
		{
			foreach (KeyValuePair<int,Entity> pair in _entities)
			{
				switch (pair.Value.Type)
				{
					case EntityType.Dust:
						PoolManager<Dust>.Instance.Release(pair.Value as Dust, pair.Value.Id);
						break;
					case EntityType.Planet:
						PoolManager<Planet>.Instance.Release(pair.Value as Planet, pair.Value.Id);
						break;
					case EntityType.Star:
						PoolManager<Star>.Instance.Release(pair.Value as Star, pair.Value.Id);
						break;
					 case EntityType.BlackHole:
						 PoolManager<BlackHole>.Instance.Release(pair.Value as BlackHole, pair.Value.Id);
						 break;
				}
			}
			_entities.Clear();
			_entityTypeLists.Clear();
			_entityBestCount = 0;
		}

		public Entity Get<T>(int serial) where T : Entity
		{
			if (_entities.ContainsKey(serial) == false)
			{
				return null;
			}

			return _entities[serial];
		}

		public List<Entity> GetAll(EntityType type)
		{
			if (_entityTypeLists.ContainsKey(type) == false)
			{
				return new List<Entity>();
			}

			return new List<Entity>(_entityTypeLists[type]);
		}
	}
}