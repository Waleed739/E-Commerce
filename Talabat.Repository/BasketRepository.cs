using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis )
        {
            _database=redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string basketid)
        {
            return await _database.KeyDeleteAsync(basketid);
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketid)
        {
            var basket = await _database.StringGetAsync(basketid);
        //    if (basket.IsNull)
        //        return null;
        //    return JsonSerializer.Deserialize<CustomerBasket>(basket);

            return basket.IsNull?null : JsonSerializer.Deserialize<CustomerBasket>(basket);

        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var createdOrUpdated = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(1));
            if (!createdOrUpdated) return null;
            return await GetBasketAsync(basket.Id);
        }
    }
}
