using Ihc.WebApi.Model;
using Microsoft.Extensions.Caching.Memory;

namespace Ihc.WebApi.Services;

public interface IAuthCacheService
{
    IAuthToken GetAuthToken();

    void ClearCache();
}

public class AuthCacheService(IAuthService authService, IMemoryCache cache)
    : IAuthCacheService
{
    private const string CacheKey = "AuthCacheService_Token";

    private static readonly MemoryCacheEntryOptions cacheOptions = new()
    {
        Priority = CacheItemPriority.High,
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
    };

    /// <summary>
    /// Ge
    /// </summary>
    /// <returns></returns>
    public IAuthToken GetAuthToken()
    {
        cache.TryGetValue(CacheKey, out AuthToken? authToken);

        // Token is valid but expired.
        if (authToken?.Token != null && authToken.ExpireTime < DateTime.UtcNow)
        {
            authService.Logout(authToken.Token);
            authToken.Token = null;
            authToken.User.AuthToken = null;
            authToken.LogoutTime = DateTime.UtcNow;
            cache.Set(CacheKey, authToken, cacheOptions);
        }

        // No token. Get a new one.
        if (authToken?.Token == null)
        {
            var user = authService.Login();
            authToken = new AuthToken
            {
                Id = Guid.NewGuid(),
                LoginTime = DateTime.UtcNow,
                ExpireTime = DateTime.UtcNow.AddMinutes(20),
                Token = user.AuthToken,
                User = user
            };
            cache.Set(CacheKey, authToken, cacheOptions);
        }

        return authToken;
    }

    /// <inheritdoc/>
    public void ClearCache()
    {
        cache.TryGetValue(CacheKey, out AuthToken? authToken);
        if (authToken?.Token != null)
        {
            authService.Logout(authToken.Token);
            authToken.Token = null;
            authToken.User.AuthToken = null;
            authToken.LogoutTime = DateTime.UtcNow;
            cache.Set(CacheKey, authToken, cacheOptions);
        }
    }
}
