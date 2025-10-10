using System;
using System.Collections.Generic;
using System.Linq;

namespace LayoutRenamer.Core.Naming
{
    // Builds a layout name from sheet number and title lines.
    // This class is independent of AutoCAD and only handles string log
    public static class NameBuilder
    {
        public static string Build(string? sheetNumber, IEnumerable<string?> titleLines)
        {
            string title = string.Join(" ", titleLines
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(titleLines => titleLines!.Trim()));

            if (!string.IsNullOrWhiteSpace(sheetNumber) && !string.IsNullOrWhiteSpace(title))
                return $"{sheetNumber.Trim()} {title}";

            if (!string.IsNullOrWhiteSpace(title))
                return title;

            return string.Empty;
        }
    }
}