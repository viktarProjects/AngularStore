using AngularStore.Core.Entities;
using AngularStore.Core.Entities.OrderAggregate;
using AngularStore.Core.Interfaces;
using AngularStore.Core.Specifications;

namespace AngularStore.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

            return await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address address)
        {
            var spec = new BasketWithItemsSpecefications(basketId);

            var basketRepository = _unitOfWork.Repository<Basket>();

            var basket = await basketRepository.GetEntityWithSpecAsync(spec);

            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                var orderItem = new OrderItem(productItem.Id, productItem.Name, productItem.PictureUrl, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var subtotal = items.Sum(item => item.Price * item.Quantity);

            var orderSpec = new OrderByPaymentIntentSpecification(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(orderSpec);

            if(existingOrder != null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.PaymentIntentId);
            }

            var order = new Order(buyerEmail, address, deliveryMethod, subtotal, items,basket.PaymentIntentId);

            var orderRepository = _unitOfWork.Repository<Order>();

            orderRepository.Add(order);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            basketRepository.Delete(basket);

            await _unitOfWork.Complete();

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }
    }
}
