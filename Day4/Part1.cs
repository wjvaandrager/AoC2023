public static class Part1
{
    public static long Value(string fileName)
    {
        var cards = new Dictionary<int, HashSet<int>>();
        var winningNumbers = new Dictionary<int, HashSet<int>>();

        var score = 0;

        using (var sr = new StreamReader(fileName))
        {
            var str = sr.ReadToEnd();
            var strList = str.Split("\r\n").ToList();
            for(var i = 0; i < strList.Count; i++)
            {
                var parts = strList[i].Split(":")[1].Trim().Split("|");
                var nums = parts[0].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToHashSet();
                var cs = parts[1].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToHashSet();

                cards.Add(i, cs);
                winningNumbers.Add(i, nums);
            }
        }

        for (int i = 0; i < cards.Count; i++)
        {
            var cardRound = cards[i];
            var numberRound = winningNumbers[i];

            var count = numberRound.Intersect(cardRound).Count();
            if(count > 0)
            {
                score += (int)Math.Pow(2, count - 1);
            }
        }

        return score;
    }
}