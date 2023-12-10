public static class Part1
{
    public static long Value(string fileName)
    {
        var now = DateTime.UtcNow.Ticks;
        var pipes = new char[140, 140];
        var startX = 0;
        var startY = 0;

        using (var sr = new StreamReader(fileName))
        {
            var str = sr.ReadToEnd();
            var strList = str.Split("\r\n").ToList();

            now = DateTime.UtcNow.Ticks;

            for (var i = 0; i < strList.Count; i++)
            {
                for (var j = 0; j < strList[i].Length; j++)
                {
                    var c = strList[i][j];
                    pipes[j, i] = c;
                    if (c == 'S')
                    {
                        startX = j;
                        startY = i;
                    }
                }
            }
        }

        var steps = 1;
        var nextX = startX;
        var nextY = startY + 1;
        var dir = 1;
        var xDir = false;

        while (nextX != startX || nextY != startY)
        {
            steps++;
            var c = pipes[nextX, nextY];

            switch (c)
            {
                case '|':
                case '-':
                    break;
                case 'F' when xDir:
                    xDir = !xDir;
                    dir = 1;
                    break;
                case 'F' when !xDir:
                    xDir = !xDir;
                    dir = 1;
                    break;
                case '7' when xDir:
                    xDir = !xDir;
                    dir = 1;
                    break;
                case '7' when !xDir:
                    xDir = !xDir;
                    dir = -1;
                    break;
                case 'J' when xDir:
                    xDir = !xDir;
                    dir = -1;
                    break;
                case 'J' when !xDir:
                    xDir = !xDir;
                    dir = -1;
                    break;
                case 'L' when xDir:
                    xDir = !xDir;
                    dir = -1;
                    break;
                case 'L' when !xDir:
                    xDir = !xDir;
                    dir = 1;
                    break;
            }

            if (xDir)
            {
                nextX += dir;
            }
            else
            {
                nextY += dir;
            }
        }

        Console.WriteLine($"{nameof(Part1)} {fileName}: {(DateTime.UtcNow.Ticks - now) / 10000} ms");
        return steps / 2;
    }
}