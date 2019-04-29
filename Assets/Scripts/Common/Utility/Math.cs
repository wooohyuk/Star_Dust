using System;
using System.Collections.Generic;

namespace Common.Utility
{
    public class Math
    {
        public static T Clamp<T>(T value, T max, T min) where T : System.IComparable<T>
        {
            if (value.CompareTo(min) < 0)
            {
                return min;
            }

            if (value.CompareTo(max) > 0)
            {
                return max;
            }

            return value;

        }

        public static bool IsValid(float x)
        {
            return !(float.IsNaN(x) || float.IsNegativeInfinity(x) || float.IsPositiveInfinity(x));
        }
        public static int RoundToInt(float x)
        {
            return Convert.ToInt32(System.Math.Round(x, 0, System.MidpointRounding.ToEven));
        }

        static Random _random = new Random(System.DateTime.Now.Second);
        //Math는 아니긴 하지만 귀찮으니 여기다 둠;
        public static void Shuffle<T>(List<T> list)
        {
            int count = list.Count;
            for (int i = 0; i < count; ++i)
            {
                int r = i + _random.Next(count - i);
                T t = list[r];
                list[r] = list[i];
                list[i] = t;
            }
        }
    }
}
