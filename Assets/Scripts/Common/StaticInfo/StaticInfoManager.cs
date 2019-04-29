using Common.StaticData;
using System.Collections.Generic;
using System.Reflection;

namespace Common.StaticInfo
{
    public sealed class StaticInfoManager
    {
        private static volatile StaticInfoManager _instance;
        private static readonly object SyncRoot = new object();
        public string ResourcePath;
        public static StaticInfoManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new StaticInfoManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private List<IStaticInfo> _staticInfos = new List<IStaticInfo>();
        private List<ISpecificInfo> _specificInfos = new List<ISpecificInfo>();
        private List<IStaticTableInfo> _staticTableInfos = new List<IStaticTableInfo>();


        private readonly StaticInfo<string, EntityInfo> _entityInfo = new StaticInfo<string, StaticData.EntityInfo>("EntityInfo.xml");        
        private readonly StaticInfo<string, ShakeInfo> _shakeInfo = new StaticInfo<string, StaticData.ShakeInfo>("ShakeInfo.xml");

        private readonly SpecificInfo<GameRuleInfo> _gameRuleInfo = new SpecificInfo<GameRuleInfo>("GameRuleInfo.xml");

        private StaticInfoManager()
        {
            FieldInfo[] fields = typeof(StaticInfoManager).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var value = field.GetValue(this);
                if (value is IStaticInfo)
                {
                    _staticInfos.Add(value as IStaticInfo);
                }
                else if (value is ISpecificInfo)
                {
                    _specificInfos.Add(value as ISpecificInfo);
                }
                else if (value is IStaticTableInfo)
                {
                    _staticTableInfos.Add(value as IStaticTableInfo);
                }
            }
        }

        public void Init(string resourcePath, bool isBinary = false, bool truncate = true, bool isReload = false)
        {
            ResourcePath = resourcePath;

            foreach (var staticInfo in _staticInfos)
            {
                staticInfo.Init(resourcePath, isBinary, truncate, isReload);
            }

            foreach (var specificInfo in _specificInfos)
            {
                specificInfo.Init(resourcePath, isBinary, isReload);
            }

            foreach (var staticTableInfo in _staticTableInfos)
            {
                staticTableInfo.Init(resourcePath, isBinary, truncate, isReload);
            }
        }

        public StaticInfo<string, EntityInfo> EntityInfos => _entityInfo;
        public StaticInfo<string, ShakeInfo> ShakeInfos => _shakeInfo;
        public GameRuleInfo GameRuleInfo => _gameRuleInfo.Get();
    }
}

