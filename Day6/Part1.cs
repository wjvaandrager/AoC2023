using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;

public static class Part1
{
    public static long Value(string fileName)
    {
        var raceTimes = new List<long>();
        var raceDurations = new List<long>();
        var results = new List<long>();

        using (var sr = new StreamReader(fileName))
        {
            var str = sr.ReadToEnd();
            var strMat = str.Split("\r\n");
            raceTimes = strMat[0].Split(":")[1].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(long.Parse).ToList();
            raceDurations = strMat[1].Split(":")[1].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(long.Parse).ToList();
            for(var i = 0; i < raceTimes.Count; i++)
            {
                var time = (double)raceTimes[i];
                var duration = (double)raceDurations[i];

                var initialVal = Math.Ceiling(duration / time);
                var result = initialVal * (time - initialVal);

                while(result <= duration)
                {
                    initialVal = Math.Ceiling(duration / (time - initialVal - 0.01));
                    result = initialVal * (time - initialVal);
                }

                var options = (long)Math.Round(time - 2 * (initialVal) + 1);
                results.Add(options);
            }
        }

        return results.Aggregate((x, y) => x * y);
    }
}