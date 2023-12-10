public static class Part2
{
    public static long Value(string fileName)
    {
        var now = DateTime.UtcNow.Ticks;
        char[,] pipes;
        var startX = 0;
        var startY = 0;
        var sizeX = 0;
        var sizeY = 0;

        using (var sr = new StreamReader(fileName))
        {
            var str = sr.ReadToEnd();
            var strList = str.Split("\r\n").ToList();

            now = DateTime.UtcNow.Ticks;

            sizeY = strList.Count;
            sizeX = strList[0].Length;
            pipes = new char[sizeX, sizeY];

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
        var xDir = -1;
        var posInside = 1;
        var inside = new bool[sizeX, sizeY];
        var coordinates = new bool[sizeX, sizeY];
        coordinates[startX, startY] = true;

        while (nextX != startX || nextY != startY)
        {
            steps++;
            var c = pipes[nextX, nextY];
            coordinates[nextX, nextY] = true;
            var insideX = nextX;
            var insideY = nextY;

            switch (c)
            {
                case '|':
                    insideX += posInside;
                    break;
                case '-':
                    insideY += posInside;
                    break;
                case 'J':
                case 'F':
                    dir *= -1;

                    insideX += posInside;
                    insideY += posInside;

                    xDir *= -1;
                    break;
                case '7':
                case 'L':
                    insideX += -1 * xDir * posInside;
                    insideY += xDir * posInside;

                    xDir *= -1;
                    posInside *= -1;
                    break;
            }

            inside[insideX, insideY] = true;
            if (insideX != nextX && insideY != nextY)
            {
                inside[nextX, insideY] = true;
                inside[insideX, nextY] = true;
            }

            if (xDir > 0)
            {
                nextX += dir;
            }
            else
            {
                nextY += dir;
            }
        }

        for (var i = 0; i < sizeY; i++)
        {
            for (var j = 0; j < sizeX; j++)
            {
                if (inside[j, i] && coordinates[j, i])
                {
                    inside[j, i] = false;
                }
            }
        }

        // fill edges
        var fillNotEnclosed = new bool[sizeX, sizeY];
        var filledSomething = true;

        while (filledSomething)
        {
            filledSomething = false;

            for (var i = 0; i < sizeY; i++)
            {
                for (var j = 0; j < sizeX; j++)
                {
                    if (i == 0 || j == 0 || i == sizeY - 1 || j == sizeX - 1)
                    {
                        fillNotEnclosed[j, i] = true;
                    }


                    if (fillNotEnclosed[j, i] || inside[j, i])
                    {
                        continue;
                    }

                    fillNotEnclosed[j, i] = fillNotEnclosed[j - 1, i] || fillNotEnclosed[j, i - 1] || fillNotEnclosed[j + 1, i] || fillNotEnclosed[j, i + 1];
                    if (fillNotEnclosed[j, i])
                    {
                        filledSomething = true;
                    }

                }
            }
        }

        var enclosed = 0;

        for (var i = 0; i < sizeY; i++)
        {
            for (var j = 0; j < sizeX; j++)
            {
                if (!fillNotEnclosed[j, i])
                {
                    enclosed++;
                }
            }
        }

        Console.WriteLine($"{nameof(Part2)} {fileName}: {(DateTime.UtcNow.Ticks - now) / 10000} ms");
        return enclosed;
    }

    static void printMat(bool[,] matrix)
    {
        for (var i = 0; i < matrix.GetLength(1); i++)
        {
            for (var j = 0; j < matrix.GetLength(0); j++)
            {
                Console.Write(matrix[j, i] ? '1' : '0');
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}