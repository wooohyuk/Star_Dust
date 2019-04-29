using Common.Utility;

namespace Common.Logic
{
    [System.Serializable]
    public struct Vector2
    {
        [System.Xml.Serialization.XmlElement("x")]
        public float X;
        [System.Xml.Serialization.XmlElement("y")]
        public float Y;

        private static Vector2 zeroVector = new Vector2(0f, 0f);
        private static Vector2 upVector = new Vector2(0f, 1f);
        private static Vector2 downVector = new Vector2(0f, -1f);
        private static Vector2 leftVector = new Vector2(-1f, 0f);
        private static Vector2 rightVector = new Vector2(1f, 0f);
        public bool IsValid
        {
            get
            {
                return Math.IsValid(X) && Math.IsValid(Y);
            }
        }
        public Vector2 Normalized
        {
            get
            {
                Vector2 v = new Vector2(X, Y);
                float length = v.Length();
                if (length < float.Epsilon)
                {
                    return v;
                }

                float invLength = 1.0f / length;
                v.X *= invLength;
                v.Y *= invLength;

                return v;
            }
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void Set(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 Parse(string str)
        {
            string st1 = str.Substring(1, str.Length - 2);
            string[] splited = st1.Split(',');

            return new Vector2(float.Parse(splited[0]), float.Parse(splited[1]));
        }

        public float Length()
        {
            return (float)System.Math.Sqrt(X * X + Y * Y);
        }

        public float LengthSquared()
        {
            return X * X + Y * Y;
        }

        public float Normalize()
        {
            float length = Length();
            if (length < float.Epsilon)
            {
                return 0.0f;
            }

            float invLength = 1.0f / length;
            X *= invLength;
            Y *= invLength;

            return length;
        }

#if UnityEngine
        public UnityEngine.Vector2 Convert()
        {
            return new UnityEngine.Vector2(X, Y);
        }

        // 일부러 형변환 쓰지 않음. 오작동의 우려가 있으므로 ㅇㅇ
        public static UnityEngine.Vector2 Convert(Vector2 v)
        {
            return new UnityEngine.Vector2(v.X, v.Y);
        }

        public static Vector2 Convert(UnityEngine.Vector2 v)
        {
            return new Vector2(v.x, v.y);
        }
#endif

        public static Vector2 operator -(Vector2 v1)
        {
            Vector2 v = new Vector2();
            v.Set(-v1.X, -v1.Y);
            return v;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            Vector2 v = new Vector2();
            v.Set(v1.X + v2.X, v1.Y + v2.Y);
            return v;
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            Vector2 v = new Vector2();
            v.Set(v1.X - v2.X, v1.Y - v2.Y);
            return v;
        }

        public static Vector2 operator *(Vector2 v1, float a)
        {
            Vector2 v = new Vector2();
            v.Set(v1.X * a, v1.Y * a);
            return v;
        }

        public static Vector2 operator *(float a, Vector2 v1)
        {
            Vector2 v = new Vector2();
            v.Set(v1.X * a, v1.Y * a);
            return v;
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public static Vector2 Zero
        {
            get
            {
                return zeroVector;
            }
        }

        public static Vector2 Up
        {
            get
            {
                return upVector;
            }
        }

        public static Vector2 Down
        {
            get
            {
                return downVector;
            }
        }

        public static Vector2 Left
        {
            get
            {
                return leftVector;
            }
        }

        public static Vector2 Right
        {
            get
            {
                return rightVector;
            }
        }

        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static Vector2 Cross(Vector2 a, float s)
        {
            Vector2 v = new Vector2();
            v.Set(s * a.Y, -s * a.X);
            return v;
        }

        public static Vector2 Cross(float s, Vector2 a)
        {
            Vector2 v = new Vector2();
            v.Set(-s * a.Y, s * a.X);
            return v;
        }

        public static Vector2 Reflect(Vector2 vector, Vector2 normal)
        {
            Vector2 v = new Vector2();
            float dot = Dot(vector, normal);
            v.Set(vector.X - ((2f * dot) * normal.X), vector.Y - ((2f * dot) * normal.Y));
            return v;
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            Vector2 c = a - b;
            return c.Length();
        }

        public static float DistanceSquared(Vector2 a, Vector2 b)
        {
            Vector2 c = a - b;
            return Vector2.Dot(c, c);
        }

        public static Vector2 GetLocalOffset(Vector2 dir, Vector2 offset)
        {
            Vector2 r = dir * offset.Y;
            Vector2 rotateR = new Vector2(dir.Y, -dir.X);
            return r + rotateR * offset.X;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2)
            {
                return (Vector2)obj == this;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
