public static class Part1
{
    public static long Value(string fileName)
    {
        var now = DateTime.UtcNow.Ticks;
        string instructions;
        var nodes = new Dictionary<string, Node>();

        var steps = 0;

        using (var sr = new StreamReader(fileName))
        {
            var str = sr.ReadToEnd();
            var strList = str.Split("\r\n").ToList();

            now = DateTime.UtcNow.Ticks;
            instructions = strList[0];

            for (var i = 2; i < strList.Count; i++)
            {
                var parts = strList[i].Split(' ', '=', '(', ')', ',').Where(x => x.Length > 1).ToList();

                var newNode = new Node { Left = parts[1], Right = parts[2] };

                nodes.Add(parts[0], newNode);
            }
        }

        var node = nodes["AAA"];

        while (true)
        {
            foreach (var c in instructions)
            {
                steps++;

                var nextNode = c == 'L' ? node.Left : node.Right;

                if (nextNode == "ZZZ")
                {
                    Console.WriteLine($"{nameof(Part1)} {fileName}: {(DateTime.UtcNow.Ticks - now) / 10000} ms");
                    return steps;
                }

                node = nodes[nextNode];
            }
        }
    }

    class Node
    {
        public string Left { get; set; } = string.Empty;

        public string Right { get; set; } = string.Empty;
    }
}