
public static class FileImporter
{
    public static long Value(string fileName)
    {
        var seeds = new List<(long, long)>();
        var maps = new Dictionary<int, SortedList<long, SeedMap>>();
        var number = 1;

        using (var sr = new StreamReader(fileName))
        {
            var str = sr.ReadToEnd();
            var strMat = str.Split("\r\n");
            var seedParts = strMat[0].Split(": ")[1].Split(" ").Select(long.Parse).ToList();
            for( var i = 0; i < seedParts.Count; i+=2)
            {
                seeds.Add((seedParts[i], seedParts[i] + seedParts[i + 1] - 1));
            }

            for (var i = 2; i < strMat.Length; i++)
            {
                var line = strMat[i];
                if (string.IsNullOrEmpty(line))
                {
                    number++;
                    continue;
                }

                if (line.EndsWith(":"))
                {
                    continue;
                }

                if(!maps.TryGetValue(number, out var sortedList))
                {
                    sortedList = new SortedList<long, SeedMap>();
                    maps.Add(number, sortedList);
                }

                var parts = line.Split(" ").Select(long.Parse).ToList();

                var seedMap = new SeedMap(number, parts[1], parts[1] + parts[2] - 1, parts[0] - parts[1]);
                sortedList.Add(seedMap.Start, seedMap);
            }
        }

        var minSeed = long.MaxValue;
        foreach(var seed in seeds)
        {
            var newSeeds = new List<(long, long)>() { seed };

            foreach (var sortmap in maps)
            {
                var newerSeeds = new List<(long, long)>();
                foreach (var newSeed in newSeeds)
                {
                    newerSeeds.AddRange(TransformRange(sortmap.Value, newSeed.Item1, newSeed.Item2));
                }

                newSeeds = newerSeeds;
            }

            minSeed = Math.Min(minSeed, newSeeds.Select(x => x.Item1).Min());
        }

        return minSeed;
    }

    private static List<(long, long)> TransformRange(SortedList<long, SeedMap> maps, long start, long end)
    {
        var afftectedMaps = maps.Where(x => x.Value.End >= start && x.Key <= end).ToList();
        var seedsMapped = new List<(long, long)>();
        var rangesCovered = new List<(long, long)>();

        foreach (var affectedMap in afftectedMaps)
        {
            var map = affectedMap.Value;
            var mapStart = map.Start;
            var mapEnd = map.Start;

            if (map.Start >= start && map.End <= end)
            {
                seedsMapped.Add((map.Start + map.Transform, map.End + map.Transform));
                rangesCovered.Add((map.Start, map.End));
            }
            else if(map.Start <= start && map.End >= end)
            {
                seedsMapped.Add((start + map.Transform, end + map.Transform));
                rangesCovered.Add((start, end));
            }
            else if (map.Start <= start && map.End >= start)
            {
                seedsMapped.Add((start + map.Transform, map.End + map.Transform));
                rangesCovered.Add((start, map.End));

                map.End = start - 1;
            }
            else if (map.End >= end && map.Start <= end)
            {
                seedsMapped.Add((mapStart + map.Transform, end + map.Transform));
                rangesCovered.Add((mapStart, end));
            }
        }

        if (rangesCovered.Count == 0)
        {
            seedsMapped.Add((start, end));
            return seedsMapped;
        }

        var startRanges = rangesCovered.First().Item1;
        var endRanges = rangesCovered.Last().Item2;

        if (start < startRanges)
        {
            seedsMapped.Add((start, startRanges - 1));
        }

        for (int i = 0; i < rangesCovered.Count; i++)
        {
            var endRange = rangesCovered[i].Item2;
            var startNextRange = i < rangesCovered.Count - 1 ? rangesCovered[i + 1].Item1 : endRanges;

            if (startNextRange > endRange + 1)
            {
                seedsMapped.Add((endRange + 1, startNextRange - 1));
            }
        }

        if (end > endRanges)
        {
            seedsMapped.Add((endRanges + 1, end));
        }

        return seedsMapped;
    }
}


public class SeedMap(int Number, long Start, long End, long Transform)
{
    public int Number { get; set; } = Number;

    public long Start { get; set; } = Start;

    public long End { get; set; } = End;

    public long Transform { get; set; } = Transform;

    public override string ToString()
    {
        return $"({Start},{End})->{Transform}";
    }
}
