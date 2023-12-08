// See https://aka.ms/new-console-template for more information

var now = DateTime.UtcNow;

Console.WriteLine(Part1.Value("inputTest.txt"));
Console.WriteLine(Part1.Value("inputFull.txt"));
Console.WriteLine(Part2.Value("inputTest2.txt"));
Console.WriteLine(Part2.Value("inputFull.txt"));

Console.WriteLine(DateTime.UtcNow - now);
