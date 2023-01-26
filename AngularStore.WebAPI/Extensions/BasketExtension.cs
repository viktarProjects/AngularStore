using AngularStore.Core.Entities;
using AngularStore.WebAPI.Dto_s;

namespace AngularStore.WebAPI.Extensions
{
    public static class BasketExtension
    {
        public static BasketDto MapBasketDto(this Basket basket)
        {
            return new BasketDto
            {
                BuyerId = basket.BuyerId,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Price = item.Product.Price,
                    PictureUrl = "https://localhost:7087/" + item.Product.PictureUrl,
                    Type = item.Product.ProductType.Name,
                    Brand = item.Product.ProductBrand.Name,
                    Quantity = item.Quantity
                }).ToList()
            };
        }
    }
}
