public static class Part2
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
            for (var i = 0; i < strList.Count; i++)
            {
                var parts = strList[i].Split(":")[1].Trim().Split("|");
                var nums = parts[0].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToHashSet();
                var cs = parts[1].Trim().Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToHashSet();

                cards.Add(i, cs);
                winningNumbers.Add(i, nums);
            }
        }

        var winCounts = new Dictionary<int, int>();

        for (int i = 0; i < cards.Count; i++)
        {
            var cardRound = cards[i];
            var numberRound = winningNumbers[i];

            var count = numberRound.Intersect(cardRound).Count();
            winCounts.Add(i, count);
        }


        var cardCounts = new Dictionary<int, int>();
        for (int i = 0; i < winCounts.Count; i++)
        {
            cardCounts.Add(i, 1);
        }

        for (int i = 0; i < winCounts.Count; i++)
        {
            var cardWins = winCounts[i];

            for(int j = 0; j < cardWins; j++)
            {
                var num = i + j + 1;
                if(num < winCounts.Count)
                {
                    cardCounts[num] += cardCounts[i];
                }
            }
        }

        for (int i = 0; i < winCounts.Count; i++)
        {
            score += winCounts[i] * cardCounts[i] + 1;
        }

        return score;
    }
}