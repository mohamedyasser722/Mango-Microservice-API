namespace Mango.Services.CouponAPI.Cache;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task SetAsync<T>(string key, T data, int? expirationInMinutes = 5, CancellationToken cancellationToken = default);
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    IEnumerable<string> GetKeysStartingWith(string prefix);
    Task RemoveKeysStartingWith(string prefix, CancellationToken cancellationToken = default);
}
//public interface ICacheKeyManager
//{
//    IEnumerable<string> GetKeysStartingWith(string prefix);
//    Task RemoveKeysStartingWith(string prefix, CancellationToken cancellationToken = default);
//}