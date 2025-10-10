using LayoutRenamer.Core.Naming;

string? sheet = "A-101";
var titles = new[] { "Floor Plan", "Level 1" };

string layoutName = NameBuilder.Build(sheet, titles);
Console.WriteLine($"Result: {layoutName}");
