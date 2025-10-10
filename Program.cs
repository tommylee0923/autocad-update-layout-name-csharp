using System;

namespace LayoutRenamer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Layout Renamer Test");

            string sheetNumber = "A-101";
            string[] titleLines = { "1st Floor", "Plan" };

            string newName = BuildingLayoutName(sheetNumber, titleLines);
            Console.WriteLine($"New Layout name: {newName}");
        }

        static string BuildingLayoutName(string sheetNumber, string[] titleLines)
        {
            string title = string.Join(" ", titleLines);
            string rawName = $"{sheetNumber} {title}";
            string sanitized = rawName.Replace(':', ' ').Replace("-", "");
            return sanitized.Trim();
        }
    }
}