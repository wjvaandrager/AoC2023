public static class Part1
{
    public static long Value(string fileName)
    {
        var now = DateTime.UtcNow.Ticks;
        var sets = new List<List<long>>();

        using (var sr = new StreamReader(fileName))
        {
            var str = sr.ReadToEnd();
            var strList = str.Split("\r\n").ToList();

            now = DateTime.UtcNow.Ticks;

            for (var i = 0; i < strList.Count; i++)
            {
                var numbers = strList[i].Split(' ').Where(x => x.Length >= 1).Select(long.Parse).ToList();

                sets.Add(numbers);
            }
        }

        var score = 0L;

        foreach (var set in sets)
        {
            var diffs = new List<List<long>>
            {
                set
            };

            var last = diffs.Last();

            while (!last.All(x => x == 0))
            {
                var next = new List<long>();
                for (var i = 0; i < last.Count - 1; i++)
                {
                    next.Add(last[i + 1] - last[i]);
                }

                last = next;
                diffs.Add(last);
            }

            for (var i = diffs.Count - 2; i >= 0; i--)
            {
                var next = diffs[i + 1].LastOrDefault(0) + diffs[i].Last();
                diffs[i].Add(next);
            }

            score += diffs[0].Last();
        }

        Console.WriteLine($"{nameof(Part1)} {fileName}: {(DateTime.UtcNow.Ticks - now) / 10000} ms");
        return score;
    }
}