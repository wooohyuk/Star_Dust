using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Common.Utility;

namespace Common.StaticInfo
{
    public interface ISpecificInfo
    {
        void Init(string path, bool isBinary = false, bool isReload = false);
    }

    public class SpecificInfo<T> : ISpecificInfo where T : new()
    {
        private T _data;
        private readonly string _path;
        private const string BinaryExtension = ".bytes";

        public SpecificInfo(string path)
        {
            _path = path;
        }

        public void Init(string resourcePath, bool isBinary = false, bool isReload = false)
        {
            bool initFailed = false;

            string path = resourcePath + _path;

            Common.Log.Logger.Debug($"Init: {typeof(T).Name} {nameof(path)} : {path}, {nameof(isBinary)} : {isBinary}, {nameof(isReload)} : {isReload}");

            try
            {
                var isJson = path.LastIndexOf(".json", System.StringComparison.CurrentCultureIgnoreCase) != -1;
                if (isBinary == true)
                {
                    //InitFromBinary(path, isReload);
                }
                else if (isJson == true)
                {
                    //InitFromJson(path, isReload);
                }
                else
                {
                    InitFromFile(path, isReload);
                }
            }
            catch (FileNotFoundException ex)
            {
                initFailed = true;
                Common.Log.Logger.Fatal($"Resource not found : {path}, Exception Message : {ex.Message}");
            }
            catch (NullReferenceException ex)
            {
                initFailed = true;
                Common.Log.Logger.Fatal(ex.Message);
            }

            if (initFailed)
            {
                throw new ResourceLoadFailedException(path);
            }

        }
        public void InitFromFile(string path, bool isReload = false)
        {
            try
            {
                UnityEngine.TextAsset textAsset = Utility.FileUtil.LoadResource<UnityEngine.TextAsset>(path);

                XmlDeserializer serializer = new XmlDeserializer(typeof(T));
                using(StringReader reader = new StringReader(textAsset.text))
                {
                    _data = (T)serializer.Deserialize(reader);
                }
            }
            catch(System.NullReferenceException)
            {
            }
            if (_data == null)
            {
                Common.Log.Logger.Error($"SpecificInfo {typeof(T).Name} Init failed, path : {path}");
            }
        }

        public T Get()
        {
            return _data;
        }
    }
}