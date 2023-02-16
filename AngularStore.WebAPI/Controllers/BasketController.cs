using AngularStore.Core.Entities;
using AngularStore.Core.Interfaces;
using AngularStore.WebAPI.Dto_s;
using AngularStore.WebAPI.Errors;
using AngularStore.WebAPI.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AngularStore.WebAPI.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        private string GetBuyerId()
        {
            return User.Identity?.Name ?? Request.Cookies["buyerId"] ?? Guid.NewGuid().ToString();
        }

        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasketById(string buyerId)
        {
            var basket = await _basketService.GetBasketAsync(buyerId);

            if(basket != null)
            {
                return basket.MapBasketDto();
            };

            return null;
        }

        [HttpPost("add-item")]
        public async Task<ActionResult<BasketDto>> AddItem(AddItemDto addItem)
        {
            var buyerId = GetBuyerId();

            var basket = await _basketService.AddItemAsync(buyerId, addItem.ProductId, addItem.Quantity);

            if (basket == null) return BadRequest(new ApiResponse(400, "An exception while adding an item"));

            CookiesManagement(basket);

            return basket.MapBasketDto();
        }

        [HttpPost("remove-item")]
        public async Task<ActionResult<BasketDto>> RemoveItem(DeleteItemDto deleteItemDto)
        {
            var buyerId = GetBuyerId();

            var basket = await _basketService.RemoveItemAsync(buyerId,deleteItemDto.ProductId, deleteItemDto.Quantity);

            if (basket == null) return BadRequest(new ApiResponse(400, "An exception while deleting an item"));

            return basket.MapBasketDto();
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteBasket()
        {
            var buyerId = GetBuyerId();

            var result = await _basketService.DeleteBasketAsync(buyerId);

            if (result == null) return BadRequest(result);

            return Ok(result);
        }

        private void CookiesManagement(Basket basket)
        {
            if (User.Identity?.Name == null)
            {
                var cookiesOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30), SameSite = SameSiteMode.None, Secure = true, HttpOnly = false };
                Response.Cookies.Append("buyerId", basket.BuyerId, cookiesOptions);
            }
            else
        {
                Response.Cookies.Delete("buyerId");
            }
        }
    }
}
