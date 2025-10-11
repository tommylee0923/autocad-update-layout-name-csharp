using System.Text.Json;

namespace LayoutRenamer.Core.Config
{
    public static class ConfigLoader
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public static TitleBlockConfig Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Config not found: {path}");

            var json = File.ReadAllText(path);

            var cfg = JsonSerializer.Deserialize<TitleBlockConfig>(json, _jsonOptions)
                      ?? new TitleBlockConfig();

            Validate(cfg);
            return cfg;
        }

        private static void Validate(TitleBlockConfig cfg)
        {
            if (string.IsNullOrWhiteSpace(cfg.TitleBlockName))
                throw new InvalidDataException("TitleBlockName is required.");
            if (string.IsNullOrWhiteSpace(cfg.SheetNum))
                throw new InvalidDataException("SheetNum is required.");
            if (cfg.Title == null || cfg.Title.Count == 0)
                throw new InvalidDataException("Title must have at least one tag.");
        }
    }
}
