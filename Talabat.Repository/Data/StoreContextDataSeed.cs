using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggreation;

namespace Talabat.Repository.Identity.Data
{
    public static class StoreContextDataSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if (!dbContext.ProductTypes.Any())
            {
                var typeData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);
                foreach (var type in types)
                {
                    await dbContext.ProductTypes.AddAsync(type);
                }
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.ProductBrands.Any())
            {
                var brandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                foreach (var brand in brands)
                {
                    await dbContext.ProductBrands.AddAsync(brand);
                }
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Products.Any())
            {
                var productData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);

                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {
                        await dbContext.Products.AddAsync(product);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.DeliveryMethods.Any())
            {
                var deliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                if (deliveryMethods?.Count > 0)
                {
                    foreach (var deliveryMethod in deliveryMethods)
                    {
                        await dbContext.DeliveryMethods.AddAsync(deliveryMethod);
                    }

                    await dbContext.SaveChangesAsync();
                }


            }
        }
    }
}
