using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Basket.Api.Cache
{
    public class CacheService : ICacheService
    {
        private IDatabase _redisCacheDb;
        public CacheService()
        {
            _redisCacheDb = ConnectionHelper.Connection.GetDatabase();
        }

        public async Task<T> GetData<T>(string key)
        {
            var value =await _redisCacheDb.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
                return JsonSerializer.Deserialize<T>(value);
            return default(T);
        }
        public async Task<T> SetData<T>(string key, T value, DateTimeOffset expirationTime = default)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var result = await _redisCacheDb.StringSetAsync(key,JsonSerializer.Serialize(value), expiryTime);
            if(result)
                return await this.GetData<T>(key);
            else
                return await Task.FromResult(default(T));

        }
        public async Task<bool> RemoveData(string key)
        {
            var _existKey  = await _redisCacheDb.KeyExistsAsync(key);
            if(_existKey)
                return await _redisCacheDb.KeyDeleteAsync(key);
            return false;
        }


    }
}
