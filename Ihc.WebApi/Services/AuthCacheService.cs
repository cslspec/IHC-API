using Ihc.WebApi.Model;
using Microsoft.Extensions.Caching.Memory;

namespace Ihc.WebApi.Services;

/// <summary>
/// Provides access to the cached IHC authentication token.
/// </summary>
public interface IAuthCacheService
{
    /// <summary>
    /// Gets the current authentication token, logging in when no valid token is cached.
    /// </summary>
    /// <returns>The cached or newly created authentication token.</returns>
    IAuthToken GetAuthToken();

    /// <summary>
    /// Logs out the cached session and clears its authentication token.
    /// </summary>
    void ClearCache();
}

/// <summary>
/// Caches the IHC authentication token and manages its session lifecycle.
/// </summary>
/// <param name="authService">Service used to log in and out of the controller.</param>
/// <param name="cache">The memory cache used to store the current token.</param>
public class AuthCacheService(IAuthService authService, IMemoryCache cache)
    : IAuthCacheService
{
    private const string CacheKey = "AuthCacheService_Token";

    private static readonly MemoryCacheEntryOptions cacheOptions = new()
    {
        Priority = CacheItemPriority.High,
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
    };

    /// <inheritdoc />
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
