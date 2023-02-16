using System.ComponentModel.DataAnnotations;

namespace AngularStore.WebAPI.Dto_s
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }

        public List<BasketItemDto> Items { get; set; }
    }
}
