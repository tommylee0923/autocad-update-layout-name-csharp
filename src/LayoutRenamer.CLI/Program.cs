using LayoutRenamer.Core.Config;
using LayoutRenamer.Core.Naming;

Console.WriteLine("LayoutRenamer CLI (Core smoke test)");

var configPath = args.FirstOrDefault()
    ?? Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "config", "blockAttributes.example.json");

TitleBlockConfig config;
try
{
    config = ConfigLoader.Load(Path.GetFullPath(configPath));
    Console.WriteLine($"Loaded config for TB '{config.TitleBlockName}'");
}
catch (Exception ex)
{
    Console.WriteLine($"Config load failed: {ex.Message}");
    return;
}

// Simulate inputs you'd get from AutoCAD:
Console.Write("Sheet number (e.g., A101): ");
var sheet = Console.ReadLine();

Console.Write("Title line 1: ");
var t1 = Console.ReadLine();
Console.Write("Title line 2 (optional): ");
var t2 = Console.ReadLine();

var built = NameBuilder.Build(sheet, new[] { t1, t2 });
var sanitized = Sanitizer.StripIllegalChars(built);

// Simulate existing layout names to test collisions:
var existing = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
{
    "A101 ENLARGED PLANS",
    "A101 ENLARGED PLANS (2)"
};

var unique = CollisionResolver.Resolve(sanitized, existing);

Console.WriteLine($"\nBuilt name:      {built}");
Console.WriteLine($"Sanitized name:  {sanitized}");
Console.WriteLine($"Unique (final):  {unique}");
