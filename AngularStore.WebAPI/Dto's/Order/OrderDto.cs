namespace AngularStore.WebAPI.Dto_s
{
    public class OrderDto
    {
        public string BasketId { get; set; }

        public int DeliveryMethodId { get; set; }

        public AddressDto Address { get; set; }
    }
}
