using System.Linq;
using Common.StaticInfo;
using DG.Tweening;
using Logic.Entity;
using UnityEngine;

namespace Logic.Situation
{
	public class CreatePlanetSituation : AbstractSituation
	{
		public CreatePlanetSituation(Vector3 pos, bool random = false)
		{
			Common.StaticData.EntityInfo[] planetInfos = StaticInfoManager.Instance.EntityInfos.GetList()
				.Where(i => i is Common.StaticData.PlanetInfo).ToArray();
			Common.StaticData.PlanetInfo selected = planetInfos[1] as Common.StaticData.PlanetInfo;
			if (random == true)
			{
				selected = planetInfos[Random.Range(0, planetInfos.Length)] as Common.StaticData.PlanetInfo;
			}
			var created = EntityManager.Instance.Create<Planet>(selected);
			created.transform.position = pos;
		}
	}
}