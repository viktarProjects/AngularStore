using AngularStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularStore.Core.Interfaces
{
    public interface IBasketService
    {
        Task<Basket> GetBasketAsync(string buyerId);

        Task<Basket> UniteBasket(string anonymBasketId, string userBasketId);

        Task<Basket> CreateBasket(string buyerId);

        Task<Basket> AddItemAsync(string buyerId, int productId, int quantity);

        Task<Basket> RemoveItemAsync(string buyerId, int productId, int quantity);

        Task<Basket> DeleteBasketAsync(string buyerId);
    }
}
