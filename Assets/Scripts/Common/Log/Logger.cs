namespace Common.Log
{
    public class Logger
    {
        public static void Debug(string str)
        {
#if UnityEngine
	        UnityEngine.Debug.Log(str);
#else
            System.Console.WriteLine(str);
#endif
        }

        public static void Log(string str)
        {
#if UnityEngine
	        UnityEngine.Debug.Log(str);
#else
            System.Console.WriteLine(str);
#endif
        }

        public static void Error(string str)
        {
#if UnityEngine
	        UnityEngine.Debug.LogError(str);
#else
            System.Console.WriteLine(str);
#endif
        }

        public static void Fatal(string str)
        {
#if UnityEngine
	        UnityEngine.Debug.LogError(str);
#else
            System.Console.WriteLine(str);
#endif
        }

        public static void Exception(System.Exception exception, string str = "")
        {
#if UnityEngine
	        UnityEngine.Debug.LogError(str);
#else
            System.Console.WriteLine(exception);
            System.Console.WriteLine(str);
#endif
        }

        public static void LogException(string str)
        {
#if UnityEngine
	        UnityEngine.Debug.LogError(str);
#else
            System.Console.WriteLine(str);
#endif
        }
    }
}
