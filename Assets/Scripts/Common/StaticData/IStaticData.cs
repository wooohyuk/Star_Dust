using System.Xml.Serialization;

namespace Common.StaticData
{
    public interface IStaticData<T>
    {
        T Key
        {
            get;
        }
    }
    [System.Serializable]
    public class StringKeyData : IStaticData<string>
    {
        [XmlAttribute("Id")]
        public string Id;
        public string Key
        {
            get
            {
                return Id;
            }
        }
    }
    [System.Serializable]
    public class IntegerKeyData : IStaticData<int>
    {
        [XmlAttribute("Id")]
        public int Id;
        public int Key
        {
            get
            {
                return Id;
            }
        }
    }
}