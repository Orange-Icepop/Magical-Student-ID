using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomNumber.Models
{
    internal static class RandomService
    {
        public static List<int> StartRandom(int min, int max, IEnumerable<int> skipNums, uint count = 1, bool skipRepeat = false)
        {
            var skips = new HashSet<int>(skipNums);
            var random = new Random();
            var result = new List<int>();
            while (result.Count < count)
            {
                var num = random.Next(min, max + 1);
                if (skips.Contains(num)) continue;
                if (skipRepeat && result.Contains(num)) continue;
                result.Add(num);
            }
            return result;
        }

    }
}
