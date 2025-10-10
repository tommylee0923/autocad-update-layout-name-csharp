using LayoutRenamer.Core.Naming;

Console.WriteLine("Testing Sanitizers:");

string input = @"A:101 / Floor?Plan  Level 1";
string result = Sanitizer.StripIllegalChars(input);

Console.WriteLine($"Input : {input}");
Console.WriteLine($"Output: {result}");
