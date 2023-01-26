using AngularStore.Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularStore.Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail,int deliveryMethod, string basketId,Address address);

        Task<IReadOnlyList<Order>> GetOrdersAsync(string buyerEmail);

        Task<Order> GetOrderByIdAsync(int id,string buyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
