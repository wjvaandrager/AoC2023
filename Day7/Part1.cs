public static class Part1
{
    public static List<char> Cards = new() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
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
            var strMat = str.Split("\r\n").ToList();
            for (var i = 0; i < strMat.Count; i++)
            {
                var hand = strMat[i].Split(" ")[0].Trim();
                var bid = strMat[i].Split(" ")[1].Trim();
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
            if (counts.Any(x => x.Count() == 5))
            {
                return Hands.FiveKind;
            }

            if (counts.Any(x => x.Count() == 4))
            {
                return Hands.FourKind;
            }

            if (counts.Any(x => x.Count() == 3) && counts.Any(x => x.Count() == 2))
            {
                return Hands.FullHouse;
            }

            if (counts.Any(x => x.Count() == 3))
            {
                return Hands.ThreeKind;
            }

            if (counts.Count() == 3)
            {
                return Hands.TwoPair;
            }

            if (counts.Any(x => x.Count() == 2))
            {
                return Hands.Pair;
            }

            return Hands.Nothing;
        }
    }
}