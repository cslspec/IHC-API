namespace Ihc.WebApi.Extensions
{
    public static class StringExtensions
    {
        public static string? Clean(this string? value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            if (string.IsNullOrWhiteSpace(value))
                return null;

            return value.Trim();
        }
    }
}
