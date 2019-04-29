using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml;

namespace Common.StaticData
{
	[Serializable]
	[XmlType("DustInfo")]
	public class DustInfo : EntityInfo
	{
	}

	[Serializable]
	[XmlType("PlanetInfo")]
	public class PlanetInfo : EntityInfo
	{
		[Serializable]
		public class GrowthInfo
		{
			[XmlAttribute]
			public int Level;
			public float Scale;
			public int RequireStarDust;
			public int DestroyDustCount;
		}
		[XmlElement("Groth")]
		public List<GrowthInfo> Growths = new List<GrowthInfo>();
	}

	[Serializable]
	[XmlType("StarInfo")]
	public class StarInfo : EntityInfo
	{
		[Serializable]
		public class GrowthInfo
		{
			[XmlAttribute]
			public int Level;
			public float Scale;
			public int RequireStarDust;
			public int DestroyDustCount;
		}
		[XmlElement("Groth")]
		public List<GrowthInfo> Growths = new List<GrowthInfo>();
	}

	[Serializable]
	[XmlType("BlackHoleInfo")]
	public class BlackHoleInfo : EntityInfo
	{
	}

	[Serializable]
	[XmlType("EntityInfo")] // define Type
	[
		XmlInclude(typeof(DustInfo)),
		XmlInclude(typeof(PlanetInfo)),
		XmlInclude(typeof(StarInfo)),
		XmlInclude(typeof(BlackHoleInfo)),
	]
	public abstract class EntityInfo : StringKeyData
	{
		[Serializable]
		public class MoveInfo
		{
			[XmlAttribute]
			public int Level;
			public float DefaultMovingSpeed;
			public float MaxMovingSpeed;
			public float LevelSpeedRate;
		}
		[XmlElement("Move")]
		public List<MoveInfo> Moves = new List<MoveInfo>(); 
	}
}
