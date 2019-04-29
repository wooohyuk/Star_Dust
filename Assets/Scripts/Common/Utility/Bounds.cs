using Common.Logic;

namespace Common.Utility
{
    public class Bounds
    {
        private Vector2 _min;
        private Vector2 _max;

        private static float skinWidth = 0.01f;

        public Bounds()
        {
            _min = new Vector2(float.MaxValue, float.MaxValue);
            _max = new Vector2(float.MinValue, float.MinValue);
        }

        public void SetPosition(Vector2 position, float offset = 0.0f)
        {
            if (_min.X > position.X)
            {
                _min.X = position.X + offset + skinWidth;
            }

            if (_min.Y > position.Y)
            {
                _min.Y = position.Y + offset + skinWidth;
            }

            if (_max.X < position.X)
            {
                _max.X = position.X - offset - skinWidth;
            }

            if (_max.Y < position.Y)
            {
                _max.Y = position.Y - offset - skinWidth;
            }
        }

        public Vector2 GetClampPosition(Vector2 position, float size)
        {
            Vector2 result = position;
            if (_min.X > position.X - size)
            {
                result.X = _min.X + size;
            }

            if (_min.Y > position.Y - size)
            {
                result.Y = _min.Y + size;
            }

            if (_max.X < position.X + size)
            {
                result.X = _max.X - size;
            }

            if (_max.Y < position.Y + size)
            {
                result.Y = _max.Y - size;
            }
            return result;
        }

        public bool Inner(Vector2 position)
        {
            if (_min.X > position.X)
            {
                return false;
            }

            if (_min.Y > position.Y)
            {
                return false;
            }

            if (_max.X < position.X)
            {
                return false;
            }

            if (_max.Y < position.Y)
            {
                return false;
            }

            return true;
        }
    }
}