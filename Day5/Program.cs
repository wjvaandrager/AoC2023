// See https://aka.ms/new-console-template for more information
//Console.WriteLine(FileImporter.Value("inputFull.txt"));

var now = DateTime.UtcNow;
Console.WriteLine(FileImporter.Value("inputFull.txt"));

Console.WriteLine(DateTime.UtcNow - now);
