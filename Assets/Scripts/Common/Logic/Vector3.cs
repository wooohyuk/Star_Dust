using Common.Utility;

namespace Common.Logic
{
    [System.Serializable]
    public struct Vector3
    {
        [System.Xml.Serialization.XmlElement("x")]
        public float X;
        [System.Xml.Serialization.XmlElement("y")]
        public float Y;
        [System.Xml.Serialization.XmlElement("z")]
        public float Z;

        private static Vector3 zeroVector = new Vector3(0f, 0f, 0f);
        private static Vector3 upVector = new Vector3(0f, 1f, 0f);
        private static Vector3 downVector = new Vector3(0f, -1f, 0f);
        private static Vector3 leftVector = new Vector3(-1f, 0f, 0f);
        private static Vector3 rightVector = new Vector3(1f, 0f, 0f);
        private static Vector3 frontVector = new Vector3(0f, 0f, 1f);
        private static Vector3 backVector = new Vector3(0f, 0f, -1f);

        public bool IsValid
        {
            get
            {
                return Math.IsValid(X) && Math.IsValid(Y) && Math.IsValid(Z);
            }
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Set(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public static Vector3 Parse(string str)
        {
            string st1 = str.Substring(1, str.Length - 2);
            string[] splited = st1.Split(',');

            return new Vector3(float.Parse(splited[0]), float.Parse(splited[1]), float.Parse(splited[2]));
        }
        public float Length()
        {
            return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
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
            Z *= invLength;

            return length;
        }

        public UnityEngine.Vector3 Convert()
        {
            return new UnityEngine.Vector3(X, Y, Z);
        }

        public static UnityEngine.Vector3 Convert(Vector3 v)
        {
            return new UnityEngine.Vector3(v.X, v.Y, v.Z);
        }

        public static Vector3 Convert(UnityEngine.Vector3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static Vector3 operator -(Vector3 v1)
        {
            Vector3 v = new Vector3();
            v.Set(-v1.X, -v1.Y, -v1.Z);
            return v;
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            Vector3 v = new Vector3();
            v.Set(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
            return v;
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            Vector3 v = new Vector3();
            v.Set(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
            return v;
        }

        public static Vector3 operator *(Vector3 v1, float a)
        {
            Vector3 v = new Vector3();
            v.Set(v1.X * a, v1.Y * a, v1.Z * a);
            return v;
        }

        public static Vector3 operator *(float a, Vector3 v1)
        {
            Vector3 v = new Vector3();
            v.Set(v1.X * a, v1.Y * a, v1.Z * a);
            return v;
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public static Vector3 Zero
        {
            get
            {
                return zeroVector;
            }
        }

        public static Vector3 Up
        {
            get
            {
                return upVector;
            }
        }

        public static Vector3 Down
        {
            get
            {
                return downVector;
            }
        }

        public static Vector3 Left
        {
            get
            {
                return leftVector;
            }
        }

        public static Vector3 Right
        {
            get
            {
                return rightVector;
            }
        }

        public static Vector3 Forward
        {
            get
            {
                return frontVector;
            }
        }

        public static Vector3 Back
        {
            get
            {
                return backVector;
            }
        }

        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        // cross는 귀찮아서 구현안함. 필요하면 하자.

        public static float Distance(Vector3 a, Vector3 b)
        {
            Vector3 c = a - b;
            return c.Length();
        }

        public static float DistanceSquared(Vector3 a, Vector3 b)
        {
            Vector3 c = a - b;
            return Vector3.Dot(c, c);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3)
            {
                return (Vector3)obj == this;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
