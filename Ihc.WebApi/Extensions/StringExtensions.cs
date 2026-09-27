namespace Ihc.WebApi.Extensions
{
    /// <summary>
    /// Provides extension methods for string values.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Trims a string and returns <see langword="null"/> when it is null, empty, or whitespace.
        /// </summary>
        /// <param name="value">The string to clean.</param>
        /// <returns>The trimmed string, or <see langword="null"/> when it contains no non-whitespace characters.</returns>
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
