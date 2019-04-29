using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Common.Utility;

namespace Common.StaticInfo
{
    public interface IStaticInfo
    {
        void Init(string resourcePath, bool isBinary = false, bool truncate = true, bool isReload = false);
    }

    public class StaticInfo<Key_T, Data_T> : IStaticInfo where Data_T : StaticData.IStaticData<Key_T>
    {
        protected Dictionary<Key_T, Data_T> Infos = new Dictionary<Key_T, Data_T>();
        private readonly string _path;
        private const string BinaryExtension = ".bytes";

        public StaticInfo(string path)
        {
            _path = path;
        }

        public int Count()
        {
            return Infos.Count;
        }

        public Data_T this[Key_T key]
        {
            get
            {
                if (Infos.ContainsKey(key) == false)
                {
                    Common.Log.Logger.Fatal($"StaticInfo {typeof(Data_T)} not found. Name : {key}");
                    return default(Data_T);
                }

                return Infos[key];
            }

            set { Infos.Add(key, value); }
        }

        public bool Exist(Key_T key)
        {
            return Infos.ContainsKey(key);
        }

        public Dictionary<Key_T, Data_T>.ValueCollection GetList()
        {
            return Infos.Values;
        }

        public void Init(string resourcePath, bool isBinary = false, bool truncate = true, bool isReload = false)
        {
            string path = resourcePath + _path;
            bool initFailed = false;

            Common.Log.Logger.Debug($"Init: {typeof(Data_T).Name} {nameof(path)} : {path}, {nameof(isBinary)} : {isBinary}, {nameof(truncate)} : {truncate}, {nameof(isReload)} : {isReload}");

            try
            {
                var isJson = path.LastIndexOf(".json", StringComparison.CurrentCultureIgnoreCase) != -1;
                if (isBinary == true)
                {
                    //InitFromBinary(path, truncate, isReload);
                }
                else if (isJson)
                {
                    //InitFromJson(path, truncate, isReload);
                }
                else
                {
                    InitFromFile(path, truncate, isReload);
                }
            }
            catch (FileNotFoundException ex)
            {
                initFailed = true;
                Common.Log.Logger.Fatal($"Resource not found : {path}. Exception Message : {ex.Message}");
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

        public void InitFromFile(string filePath, bool truncate, bool isReload)
        {
            if (truncate)
            {
                Infos.Clear();
            }

            List<Data_T> infoList = null;

            try
            {
                UnityEngine.TextAsset textAsset = Utility.FileUtil.LoadResource<UnityEngine.TextAsset>(filePath);
                XmlDeserializer serializer = new XmlDeserializer(typeof(List<Data_T>));
                using (StringReader reader = new StringReader(textAsset.text))
                {
                    infoList = (List<Data_T>) serializer.Deserialize(reader);
                }
            }
            catch(System.NullReferenceException)
            {
                throw;
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            if (infoList != null)
            {
                foreach (Data_T info in infoList)
                {
                    AddInfo(info, isReload);
                }
            }
        }

        public void InitFromUrl(string url, bool truncate = true)
        {
            if (truncate)
            {
                Infos.Clear();
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(url);

            List<Data_T> infoList = null;

            XmlDeserializer serializer = new XmlDeserializer(typeof(List<Data_T>));
            using (StringReader reader = new StringReader(doc.OuterXml))
            {
                infoList = (List<Data_T>)serializer.Deserialize(reader);
            }

            foreach (Data_T info in infoList)
            {
                AddInfo(info, false);
            }
        }

        protected virtual void PreAddInfo(Data_T info)
        {
        }

        void AddInfo(Data_T info, bool isReload)
        {
            PreAddInfo(info);

            if (isReload == true)
            {
                if (Infos.ContainsKey(info.Key))
                {
                    Utility.ObjectExtensions.Replace(info, Infos[info.Key]);
                }
                else
                {
                    Infos.Add(info.Key, info);
                }
            }
            else
            {
                try
                {
                    Infos.Add(info.Key, info);
                }
                catch (System.ArgumentException e)
                {
                    Common.Log.Logger.Fatal("Duplicated key : " + info.Key + ", " + e);
                    throw;
                }
            }
        }

        public void SimpleInit(string filePath, bool truncate = true)
        {
            if (truncate)
            {
                Infos.Clear();
            }

            Data_T info = default(Data_T);
            bool loaded = false;

            try
            {
                UnityEngine.TextAsset textAsset = Utility.FileUtil.LoadResource<UnityEngine.TextAsset>(filePath);
                XmlDeserializer serializer = new XmlDeserializer(typeof(Data_T));
                using (StringReader reader = new StringReader(textAsset.text))
                {
                    info = (Data_T) serializer.Deserialize(reader);
                    loaded = true;
                }
            }
            catch(System.NullReferenceException)
            {
            }
            if (loaded == true)
            {
                Infos.Add(info.Key, info);
            }
        }

        public void InitList(string listFilePath)
        {
            List<string> infoPathList = null;

            try
            {
                UnityEngine.TextAsset textAsset = Utility.FileUtil.LoadResource<UnityEngine.TextAsset>(listFilePath);
                XmlDeserializer serializer = new XmlDeserializer(typeof(List<string>));
                using (StringReader reader = new StringReader(textAsset.text))
                {
                    infoPathList = (List<string>) serializer.Deserialize(reader);

                    foreach (string path in infoPathList)
                    {
                        SimpleInit(path, false);
                    }
                }
            }
            catch(System.NullReferenceException)
            {
            }
        }

        public void Write(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var src = File.CreateText(filePath);

            List<Data_T> infoList = new List<Data_T>();

            foreach (Data_T info in Infos.Values)
            {
                infoList.Add(info);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<Data_T>));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, infoList);
                src.Write(writer.ToString());
            }

            src.Close();
        }
    }

    public class ResourceLoadFailedException : Exception
    {
        public ResourceLoadFailedException(string path)
            : base($"Resource load failed : {path}")
        {
        }
    }
}