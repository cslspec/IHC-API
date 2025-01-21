namespace Ihc.WebApi.Util;

public static class Clean
{
    public static int? Int(int value)
    {
        if (value <= 0)
            return null;

        return value;
    }
}
