using System;
using System.Linq;

namespace KustoSdkTests
{
    public class TelemetryGenerator
    {
        public string GenerateRandomString(int minLength = 5, int maxLength = 50)
        {
            var s= Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var length = GenerateRandomInt(minLength, maxLength);

            if (length < s.Length)
            {
                return s.Substring(0, length);
            }

            return string.Concat(Enumerable.Repeat(s, (length / s.Length) + 1)).Substring(0, length);

        }

        public int GenerateRandomInt(int lo = int.MinValue, int hi = int.MaxValue)
        {
            return new Random().Next(lo, hi);
        }

        public float GenerateRandomFloat(float lo = float.MinValue, float hi = float.MaxValue)
        {
            var d =  lo + (new Random().NextDouble() * (hi - lo));
            return (float) d;
        }

        public DateTime GenerateRandomDateTime(int withinLastMinutes=30)
        {
            var now = DateTime.Now;
            var r = GenerateRandomInt(0, withinLastMinutes);
            return now.AddMinutes(-r);
        }
    }
}
