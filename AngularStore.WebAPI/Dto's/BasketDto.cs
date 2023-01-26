namespace AngularStore.WebAPI.Dto_s
{
    public class BasketDto
    {
        public string BuyerId { get; set; }

        public List<BasketItemDto> Items { get; set; }

        public int? DeliveryMethodId { get; set; }

        public string? ClientSecret { get; set; }

        public string? PaymentIntentId { get; set; }

        public decimal ShippingPrice { get; set; }
    }
}
