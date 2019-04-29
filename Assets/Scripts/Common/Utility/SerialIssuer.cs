using System.Threading;

namespace Common.Utility
{
    public class SerialIssuer
    {
        private int _lastIssuedSerial;

        public SerialIssuer() : this(0)
        {
        }

        public SerialIssuer(int initSerial)
        {
            _lastIssuedSerial = initSerial;
        }

        public int Issue()
        {
            int incremented = Interlocked.Increment(ref _lastIssuedSerial);
            return incremented;
        }
    }
}