using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularStore.Core.Entities
{
    public class Basket : BaseEntity
    {
        public string BuyerId { get; set; }

        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public int? DeliveryMethodId { get; set;}

        public string? ClientSecret { get; set; }

        public string? PaymentIntentId { get; set; }

        public decimal ShippingPrice { get; set; }

        public void AddItem(int productId, int quantity)
        {
            var item = Items.Find(x => x.ProductId == productId);

            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                var basketItem = new BasketItem()
                {
                    ProductId = productId,
                    Quantity = quantity
                };

                Items.Add(basketItem);
            }
        }

        public void RemoveItem(int productId, int quantity)
        {
            var item = Items.Find(x => x.ProductId == productId);

            if (item != null)
            {
                item.Quantity -= quantity;

                if (item.Quantity <= 0)
                {
                    Items.Remove(item);
                }
            }
        }
    }
}
