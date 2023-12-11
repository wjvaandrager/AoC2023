public static class Part1
{
    public static long Value(string fileName)
    {
        var now = DateTime.UtcNow.Ticks;
        var map = new List<List<char>>();

        using (var sr = new StreamReader(fileName))
        {
            var str = sr.ReadToEnd();
            var strList = str.Split("\r\n").ToList();

            now = DateTime.UtcNow.Ticks;

            for (var i = 0; i < strList.Count; i++)
            {
                var row = strList[i].ToList();
                if (row.All(x => x == '.'))
                {
                    map.Add(new List<char>(row));
                }

                map.Add(row);
            }
        }

        var columns = new List<int>();
        for (var i = map[0].Count - 1; i >= 0; i--)
        {
            var column = map.Select(x => x[i]);
            if (column.All(x => x == '.'))
            {
                columns.Add(i);
            }
        }

        foreach (var column in columns)
        {
            for (var i = 0; i < map.Count; i++)
            {
                map[i].Insert(column, '.');
            }
        }

        var galaxies = new List<(int, int)>();
        for (var i = 0; i < map.Count; i++)
        {
            for (var j = 0; j < map[i].Count; j++)
            {
                if (map[i][j] == '#')
                {
                    galaxies.Add((i, j));
                }
            }
        }

        var score = 0L;

        for (var i = 0; i < galaxies.Count; i++)
        {
            var galaxyA = galaxies[i];
            for (var j = i + 1; j < galaxies.Count; j++)
            {
                var galaxyB = galaxies[j];
                var dist = Math.Abs(galaxyA.Item1 - galaxyB.Item1) + Math.Abs(galaxyA.Item2 - galaxyB.Item2);

                score += dist;
            }
        }

        Console.WriteLine($"{nameof(Part1)} {fileName}: {(DateTime.UtcNow.Ticks - now) / 10000} ms");
        return score;
    }
}