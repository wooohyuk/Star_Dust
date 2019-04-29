using Common.Logic;

namespace Common.StaticData
{
    [System.Serializable]
    public class ShakeInfo : StringKeyData, System.ICloneable
    {
        public float Duration;
        public float FadeInTime;
        public float FadeOutTime;
        public float Scale;
        public float Delay;
        public float Speed;
        public Vector3 PositionInfluence;
        public Vector3 RotationInfluence;

        public object Clone()
        {
            ShakeInfo newInstance = new ShakeInfo();
            newInstance.Id = this.Id;
            newInstance.Duration = this.Duration;
            newInstance.FadeInTime = this.FadeInTime;
            newInstance.FadeOutTime = this.FadeOutTime;
            newInstance.Scale = this.Scale;
            newInstance.Delay = this.Delay;
            newInstance.Speed = this.Speed;
            newInstance.PositionInfluence = this.PositionInfluence;
            newInstance.RotationInfluence = this.RotationInfluence;

            return newInstance;
        }
    }
}