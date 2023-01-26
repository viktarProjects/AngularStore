using AngularStore.Core.Entities;
using AngularStore.Core.Entities.OrderAggregate;
using AngularStore.Core.Interfaces;
using AngularStore.Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = AngularStore.Core.Entities.Product;

namespace AngularStore.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGenericRepository<Basket> _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IGenericRepository<Basket> basketRepository,IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<Basket> CreateOrUpdatePaymentIntent(string buyerId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var spec = new BasketWithItemsSpecefications(buyerId);

            var basket = await _basketRepository.GetEntityWithSpecAsync(spec);

            if (basket == null) return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync((int)basket.DeliveryMethodId);

                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                if(item.Product.Price != productItem.Price)
                {
                    item.Product.Price = productItem.Price;
                }
            }

            var service = new PaymentIntentService();

            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Product.Price * 100)) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Product.Price * 100)) + (long)shippingPrice * 100
                };

                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            _basketRepository.Update(basket);

            return basket;
        }

        public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);

            if (order == null) return null;

            order.Status = OrderStatus.PaymentReceived;
            _unitOfWork.Repository<Order>().Update(order);
              
            await _unitOfWork.Complete();

            return order;
        }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);

            if (order == null) return null;

            order.Status = OrderStatus.PaymentFailed;
            //_unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.Complete();

            return order;

        }
    }
}
