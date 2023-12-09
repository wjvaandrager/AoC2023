public static class Part2
{
    public static long Value(string fileName)
    {
        var now = DateTime.UtcNow.Ticks;
        string instructions;
        var nodes = new Dictionary<string, Node>();

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

        var startNodes = nodes.Keys.Where(x => x.EndsWith('A')).ToList();
        var stepCounts = new List<int>();

        foreach (var startNode in startNodes)
        {
            var steps = 0;

            var node = nodes[startNode];
            var notFound = true;

            while (notFound)
            {
                foreach (var c in instructions)
                {
                    steps++;

                    var nextNode = c == 'L' ? node.Left : node.Right;

                    if (nextNode.EndsWith("Z"))
                    {
                        stepCounts.Add(steps);
                        notFound = false;
                        break;
                    }

                    node = nodes[nextNode];
                }
            }
        }

        var answer = GFG.lcm_of_array_elements(stepCounts.ToArray());

        Console.WriteLine($"{nameof(Part2)} {fileName}: {(DateTime.UtcNow.Ticks - now) / 10000} ms");

        return answer;
    }

    class Node
    {
        public string Left { get; set; } = string.Empty;

        public string Right { get; set; } = string.Empty;
    }

    // https://www.geeksforgeeks.org/lcm-of-given-array-elements/
    class GFG
    {
        public static long lcm_of_array_elements(int[] element_array)
        {
            long lcm_of_array_elements = 1;
            int divisor = 2;

            while (true)
            {

                int counter = 0;
                bool divisible = false;
                for (int i = 0; i < element_array.Length; i++)
                {

                    // lcm_of_array_elements (n1, n2, ... 0) = 0.
                    // For negative number we convert into
                    // positive and calculate lcm_of_array_elements.
                    if (element_array[i] == 0)
                    {
                        return 0;
                    }
                    else if (element_array[i] < 0)
                    {
                        element_array[i] = element_array[i] * (-1);
                    }
                    if (element_array[i] == 1)
                    {
                        counter++;
                    }

                    // Divide element_array by devisor if complete
                    // division i.e. without remainder then replace
                    // number with quotient; used for find next factor
                    if (element_array[i] % divisor == 0)
                    {
                        divisible = true;
                        element_array[i] = element_array[i] / divisor;
                    }
                }

                // If divisor able to completely divide any number
                // from array multiply with lcm_of_array_elements
                // and store into lcm_of_array_elements and continue
                // to same divisor for next factor finding.
                // else increment divisor
                if (divisible)
                {
                    lcm_of_array_elements = lcm_of_array_elements * divisor;
                }
                else
                {
                    divisor++;
                }

                // Check if all element_array is 1 indicate 
                // we found all factors and terminate while loop.
                if (counter == element_array.Length)
                {
                    return lcm_of_array_elements;
                }
            }
        }
    }
}