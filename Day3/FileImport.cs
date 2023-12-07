using System.IO;

public static class FileImporter
{
    public static string symbols = "*/#%&$@-+=";

    public static int Value(string fileName)
    {
        var numbers = new List<Number>();
        var syms = new List<Symbol>();

        using (var sr = new StreamReader(fileName))
        {
            var str = sr.ReadToEnd();
            var strMat = str.Split("\r\n");
            var number = string.Empty;

            for (var i = 0; i < strMat.Length; i++)
            {
                var row = strMat[i]; 
                for (var j = 0; j < row.Length; j++)
                {
                    var c = row[j]; 
                    if (symbols.Contains(c))
                    {
                        syms.Add(new Symbol(i, j));
                    }

                    if(int.TryParse(c.ToString(), out _))
                    {
                        number += c;
                    }
                    else if(!string.IsNullOrEmpty(number))
                    {
                        var value = int.Parse(number);
                        var cols = number.Select((x, idx) => j - idx - 1).ToList();
                        numbers.Add(new Number(value, i, cols));
                        number = string.Empty;
                    }
                }

                if (!string.IsNullOrEmpty(number))
                {
                    var value = int.Parse(number);
                    var cols = number.Select((x, idx) => row.Length - idx - 2).ToList();
                    numbers.Add(new Number(value, i, cols));
                    number = string.Empty;
                }
            }
        }

        foreach (var symbol in syms)
        {
            for (var i = -1; i <= 1; i++)
            {
                var row = symbol.Row + i;
                for (var j = -1; j <= 1; j++)
                {
                    var col = symbol.Col + j;
                    foreach (var number in numbers)
                    {
                        if(number.Row == row && number.Columns.Contains(col))
                        {
                            number.SymbolsHit++;
                        }
                    }
                }
            }
        }

        return numbers.Where(x => x.SymbolsHit > 0).Sum(x => x.Value);
    }

    public static int Value2(string fileName)
    {
        var numbers = new List<Number>();
        var syms = new List<Symbol>();

        using (var sr = new StreamReader(fileName))
        {
            var str = sr.ReadToEnd();
            var strMat = str.Split("\r\n");
            var number = string.Empty;

            for (var i = 0; i < strMat.Length; i++)
            {
                var row = strMat[i];
                for (var j = 0; j < row.Length; j++)
                {
                    var c = row[j];
                    if (c == '*')
                    {
                        syms.Add(new Symbol(i, j));
                    }

                    if (int.TryParse(c.ToString(), out _))
                    {
                        number += c;
                    }
                    else if (!string.IsNullOrEmpty(number))
                    {
                        var value = int.Parse(number);
                        var cols = number.Select((x, idx) => j - idx - 1).ToList();
                        numbers.Add(new Number(value, i, cols));
                        number = string.Empty;
                    }
                }

                if (!string.IsNullOrEmpty(number))
                {
                    var value = int.Parse(number);
                    var cols = number.Select((x, idx) => row.Length - idx - 2).ToList();
                    numbers.Add(new Number(value, i, cols));
                    number = string.Empty;
                }
            }
        }

        var sum = 0;

        foreach (var symbol in syms)
        {
            var numbersHit = new HashSet<Number>();
            for (var i = -1; i <= 1; i++)
            {
                var row = symbol.Row + i;
                for (var j = -1; j <= 1; j++)
                {
                    var col = symbol.Col + j;
                    foreach (var number in numbers)
                    {
                        if (number.Row == row && number.Columns.Contains(col))
                        {
                            numbersHit.Add(number);
                        }
                    }
                }
            }

            if(numbersHit.Count == 2)
            {
                sum += numbersHit.First().Value * numbersHit.Last().Value;
            }
        }

        return sum;
    }
}

public class Number
{
    public Number(int value, int row, List<int> cols)
    {
        Value = value;
        Row = row;
        Columns = cols;
    }

    public int Value { get; set; }

    public int Row { get; set; }

    public List<int> Columns { get; set; }

    public int SymbolsHit { get; set; }
}

public record Symbol(int Row, int Col);