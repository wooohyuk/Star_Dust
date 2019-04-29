using System.Linq;
using Common.StaticInfo;
using DG.Tweening;
using Logic.Entity;
using UnityEngine;

namespace Logic.Situation
{
	public class CreateBlackHoleSituation : AbstractSituation
	{
		public CreateBlackHoleSituation(Vector3 pos)
		{
			Common.StaticData.EntityInfo[] blackHoleInfo = StaticInfoManager.Instance.EntityInfos.GetList()
				.Where(i => i is Common.StaticData.BlackHoleInfo).ToArray();
			Common.StaticData.BlackHoleInfo selected = blackHoleInfo[blackHoleInfo.Length - 1] as Common.StaticData.BlackHoleInfo;
			var created = EntityManager.Instance.Create<BlackHole>(selected);
			created.transform.position = pos;
		}
	}
}