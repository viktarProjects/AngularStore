using AngularStore.Core.Entities.OrderAggregate;

namespace AngularStore.WebAPI.Dto_s
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public Address Address { get; set; }

        public string DeliveryMethod { get; set; }

        public decimal ShippingPrice { get; set; }

        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }

        public string Status { get; set; }
    }
}
