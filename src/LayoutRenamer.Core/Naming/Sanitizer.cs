
using System;
using System.Collections.Generic;
using System.Linq;

namespace LayoutRenamer.Core.Naming
{
    public static class Sanitizer
    {
        public static readonly char[] IllegalChars = ['\\', '/', ':', ';', '*', '?', '"', '<', '>', '|'];

        public static string StripIllegalChars(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            string cleaned = new string(input
            .Select(c => IllegalChars.Contains(c) ? '-' : c)
            .ToArray());

            cleaned = string.Join(" ", cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            return cleaned.Trim();
        }
    }
}