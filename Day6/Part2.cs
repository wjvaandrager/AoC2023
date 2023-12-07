using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;

public static class Part2
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
            var raceTime = strMat[0].Split(":")[1].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Aggregate((x, y) => x + y).ToString();
            raceTimes.Add(long.Parse(raceTime));
            var raceDuration = strMat[1].Split(":")[1].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Aggregate((x, y) => x + y).ToString();
            raceDurations.Add(long.Parse(raceDuration));

            for (var i = 0; i < raceTimes.Count; i++)
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