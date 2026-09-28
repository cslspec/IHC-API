namespace Ihc.WebApi.Util;

/// <summary>
/// Provides helpers for normalizing values returned by the IHC controller.
/// </summary>
public static class Clean
{
    /// <summary>
    /// Converts non-positive integers to <see langword="null"/>.
    /// </summary>
    /// <param name="value">The integer value to normalize.</param>
    /// <returns>The value when it is positive; otherwise, <see langword="null"/>.</returns>
    public static int? Int(int value)
    {
        return value <= 0 ? null : value;
    }
}
