using System.Collections.Generic;

namespace LayoutRenamer.Core.Config
{
    public sealed class TitleBlockConfig
    {
        public string TitleBlockName { get; init; } = "";
        public string SheetNum { get; init; } = "";
        public List<string> Title { get; init; } = new();
    }
}