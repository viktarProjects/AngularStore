using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularStore.Core.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {

        }

        public Order(
            string buyerEmail,
            Address address,
            DeliveryMethod deliveryMethod,
            decimal subtotal,
            IReadOnlyList<OrderItem> orderItems,
            string paymentIntentId
            )
        {
            BuyerEmail = buyerEmail;
            Address = address;
            DeliveryMethod = deliveryMethod;
            Subtotal = subtotal;
            OrderItems = orderItems;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public Address Address { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }

        public IReadOnlyList<OrderItem> OrderItems { get; set; }

        public decimal Subtotal { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string PaymentIntentId { get; set; }

        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }
    }
}
