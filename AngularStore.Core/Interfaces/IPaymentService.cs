using AngularStore.Core.Entities;
using AngularStore.Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularStore.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<Basket> CreateOrUpdatePaymentIntent(string buyerId);
        Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId);
        Task<Order> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}
