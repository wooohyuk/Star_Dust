#if UnityEngine
using UnityEngine;

namespace Common.Utility
{
    public static class FileUtil
    {
        public static T LoadResource<T>(string path) where T : UnityEngine.Object
        {
            System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
            strBuilder.Length = 0;

            T asset = null;

            strBuilder.Append(path);
            asset = (T)Resources.Load(RemoveExtension(strBuilder.ToString()));

            return asset;
        }

        public static string RemoveExtension(string path)
        {
            if(string.IsNullOrEmpty(path))
                return path;

            string result = path;
            int lastIndex = path.LastIndexOf('.');
            if(lastIndex > -1)
            {
                result = path.Substring(0, lastIndex);
            }
            return result;
        }
    }
}
#endif