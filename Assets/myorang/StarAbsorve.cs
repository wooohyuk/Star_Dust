using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logic.Entity;

public class StarAbsorve : MonoBehaviour
{
	private Star _star;
	private void Awake()
	{
		_star = transform.parent.GetComponent<Star>();
		_star.SetAbsorve(this);
	}

	private void Update()
	{
		List<Entity> stars = EntityManager.Instance.GetAll(EntityType.Star);
		for (int i = 0; i < stars.Count; ++i)
		{
			Star otherEntity = stars[i] as Star;
			if (otherEntity == _star)
			{
				continue;
			}
				
			float distance = (otherEntity.transform.position - _star.transform.position).magnitude;
			float radiuses = _star.GetRadius() + otherEntity.GetRadius();
			Debug.Log($"StarAbsorve, {_star} <-> {otherEntity}, {distance}|{radiuses}");
			if (distance - 1f > _star.GetRadius() + otherEntity.GetRadius())
			{
				return;
			}
			
			if(otherEntity.Type == _star.Type)
			{
				if(otherEntity.level == _star.level)
				{
					if (otherEntity.Serial > _star.Serial)
					{
						EntityManager.Instance.Destroy<Star>(_star);
						otherEntity.level++;
						otherEntity.OnChangeLevel();
						return;
					}
					else
					{
						EntityManager.Instance.Destroy<Star>((Star)otherEntity);
						_star.level++;
						_star.OnChangeLevel();
						return;
					}
				}
				else if (otherEntity.level < _star.level)
				{
					EntityManager.Instance.Destroy<Star>((Star)otherEntity);
					_star.level++;
					_star.OnChangeLevel();
					return;
				}
				return;
			}

		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
        Entity otherEntity = other.transform?.GetComponent<Entity>();
		if (otherEntity == null)
			return;
	    
//	    float distance = (other.transform.position - _star.transform.position).magnitude;
//	    float radiuses = _star.GetRadius() + otherEntity.GetRadius();
//	    Debug.Log($"StarAbsorve, {_star} <-> {otherEntity}, {distance}|{radiuses}");
//	    if (distance - 1f > _star.GetRadius() + otherEntity.GetRadius())
//	    {
//		    return;
//	    }

        // 상위개체와 충돌함 (Star, Blackhole)
        if (otherEntity.Type > _star.Type)
			return;

		// 동일개체와 충돌함 (Planet)
        if(otherEntity.Type == _star.Type)
        {
            if(otherEntity.level == _star.level)
            {
                if (otherEntity.Serial > _star.Serial)
                {
                    EntityManager.Instance.Destroy<Star>(_star);
                    otherEntity.level++;
                    otherEntity.OnChangeLevel();
                    return;
                }
                else
                {
                    EntityManager.Instance.Destroy<Star>((Star)otherEntity);
                    _star.level++;
                    _star.OnChangeLevel();
                    return;
                }
            }
            else if (otherEntity.level < _star.level)
            {
                EntityManager.Instance.Destroy<Star>((Star)otherEntity);
                _star.level++;
                _star.OnChangeLevel();
                return;
            }
            return;
        }

		// 하위개체와 추돌함 (Dust)
		switch (otherEntity.Type)
		{
			case EntityType.Undefined:
				break;

			case EntityType.Planet:
                Planet otherPlanet = (Planet)otherEntity;
				_star.CollectPlanet(otherPlanet);
				break;
			
			case EntityType.Dust:
				Dust dust = (Dust)otherEntity;
				_star.CollectDust(dust);
				break;


			default:
				break;
		}
	}
}
