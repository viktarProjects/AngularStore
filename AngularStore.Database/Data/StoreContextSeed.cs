using AngularStore.Core.Entities;
using AngularStore.Core.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AngularStore.Database.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../AngularStore.Database/SeedData/types.json");

                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    foreach (var type in types)
                    {
                        await context.ProductTypes.AddAsync(type);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../AngularStore.Database/SeedData/brands.json");

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    foreach (var brand in brands)
                    {
                       await context.ProductBrands.AddAsync(brand);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.Products.Any())
                {
                    var productsData = File.ReadAllText("../AngularStore.Database/SeedData/products.json");

                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    foreach (var product in products)
                    {
                        await context.Products.AddAsync(product);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.DeliveryMethods.Any())
                {
                    var deliveryMethodsData = File.ReadAllText("../AngularStore.Database/SeedData/delivery.json");

                    var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                    foreach (var item in methods)
                    {
                        await context.DeliveryMethods.AddAsync(item);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
