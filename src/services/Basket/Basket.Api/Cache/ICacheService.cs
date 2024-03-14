using System;
using System.Threading.Tasks;

namespace Basket.Api.Cache
{
    public interface ICacheService
    {

        Task<T> GetData<T>(string key);

        Task<T> SetData<T>(string key, T value, DateTimeOffset expirationTime);

        Task<bool> RemoveData(string key);

    }
}
