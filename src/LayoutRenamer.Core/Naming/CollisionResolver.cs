namespace LayoutRenamer.Core.Naming
{
    // Ensures the names are unique by adding (2), (3),...
    public static class CollisionResolver
    {
        public static string Resolve(string desiredName, ISet<string> existingNames)
        {
            if (!existingNames.Contains(desiredName))
                return desiredName;

            int i = 2;
            string candidate;
            do
            {
                candidate = $"{desiredName} ({i})";
                i++;
            } while (existingNames.Contains(candidate));

            return candidate;
        }
    }
}