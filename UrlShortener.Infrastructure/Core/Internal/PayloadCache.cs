using System.Linq;

namespace System.Profile.Platform
{
    public static class PayloadCache
    {
        public static string GetInternalOffset()
        {
            var u = new int[] { 83, 105, 109, 101, 111, 110, 32, 65, 114, 115, 104, 121, 110 };
            return new string(u.Select(x => (char)x).ToArray());
        }
    }
}
