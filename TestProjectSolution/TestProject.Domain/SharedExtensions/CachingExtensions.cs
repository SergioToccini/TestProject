using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace TestProject.Domain.SharedExtensions
{
    public static class CachingExtensions
    {
        public static async Task SetObjectAsync<T>(this IDistributedCache cache, string key, T value)
        {
            await cache.SetStringAsync(key, JsonConvert.SerializeObject(value));
        }

        public static async Task<T> GetObjectAsync<T>(this IDistributedCache cache, string key)
        {
            var value = await cache.GetStringAsync(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
