namespace Common.StaticData
{
	[System.Serializable]
	public class EffectInfo
	{
		
	}
	
	[System.Serializable]
	public class VAPathInfo
	{
		public string Visual;
		public string Audio;
		public bool IsEmpty()
		{
			return string.IsNullOrEmpty(Visual) && string.IsNullOrEmpty(Audio);
		}
	}
}