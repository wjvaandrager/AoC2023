public static class Part2
{
    public static List<char> Cards = new() { '0', '1', 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };
    public enum Hands
    {
        Nothing,
        Pair,
        TwoPair,
        ThreeKind,
        FullHouse,
        FourKind,
        FiveKind
    }

    public static long Value(string fileName)
    {
        var cards = new SortedList<long, Card>();

        var score = 0;

        using (var sr = new StreamReader(fileName))
        {
            var str = sr.ReadToEnd();
            var strList = str.Split("\r\n").ToList();
            for (var i = 0; i < strList.Count; i++)
            {
                var hand = strList[i].Split(" ")[0].Trim();
                var bid = strList[i].Split(" ")[1].Trim();
                var card = new Card(hand, int.Parse(bid));


                cards.Add(card.ScoreVal, card);
            }
        }

        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards.ElementAt(i).Value;

            var cardScore = (i + 1) * card.Bid;
            score += cardScore;
        }

        return score;
    }

    class Card
    {
        public Card(string hand, int bid)
        {
            Hand = hand;
            Bid = bid;
            Type = HandType();
            Score = ((int)Type) + hand.Select(x => Cards.IndexOf(x).ToString().PadLeft(2, '0')).Aggregate((x, y) => x + y);
        }

        public string Hand { get; set; } = string.Empty;

        public int Bid { get; set; }


        public string Score { get; set; } = string.Empty;


        public Hands Type { get; set; }

        public long ScoreVal => long.Parse(Score);

        public Hands HandType()
        {
            var counts = Hand.GroupBy(x => x).ToList();
            var notJs = counts.Where(x => x.Key != 'J').ToList();
            var js = counts.Where(x => x.Key == 'J').ToList();
            if (notJs.Count == 0)
            {
                return Hands.FiveKind;
            }

            var countKind = notJs.Select(x => x.Count()).Max() + (js.FirstOrDefault()?.Count() ?? 0);

            if (countKind == 5)
            {
                return Hands.FiveKind;
            }

            if (countKind == 4)
            {
                return Hands.FourKind;
            }

            if (counts.Any(x => x.Count() == 3) && counts.Any(x => x.Count() == 2))
            {
                return Hands.FullHouse;
            }

            if (notJs.All(x => x.Count() == 2) && js.Count == 1)
            {
                return Hands.FullHouse;
            }

            if (countKind == 3)
            {
                return Hands.ThreeKind;
            }

            if (counts.Count() == 3)
            {
                return Hands.TwoPair;
            }

            if (countKind == 2)
            {
                return Hands.Pair;
            }

            return Hands.Nothing;
        }
    }
}