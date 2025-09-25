using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Device.Fingerprint.ExternalAPI.Services
{

    public interface IRedisService
    {
        Task<long> HashIncrementAsync(string hashKey, string key, long value);
    }

    public class RedisService : IRedisService
    {
        private readonly IRedisDatabase redisDatabase;

        public RedisService(IRedisClient redisClient)
        {
            redisDatabase = redisClient.GetDefaultDatabase();
        }

        public Task<long> HashIncrementAsync(string hashKey, string key, long value)
        {
            return redisDatabase.HashIncrementByAsync(hashKey, key, value);
        }
    }
}
