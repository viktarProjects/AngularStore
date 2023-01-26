using AngularStore.Core.Entities;
using AngularStore.Core.Interfaces;
using AngularStore.Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace AngularStore.Core.Services
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BasketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Basket> GetBasketAsync(string buyerId)
        {
            var spec = new BasketWithItemsSpecefications(buyerId);

            var basketRepository = _unitOfWork.Repository<Basket>();

            return await basketRepository.GetEntityWithSpecAsync(spec);
        }

        public async Task<Basket> CreateBasket(string buyerId)
        {
            var basketRepository = _unitOfWork.Repository<Basket>();

            var basket = new Basket() { BuyerId = buyerId };

            basketRepository.Add(basket);

            return basket;
        }

        public async Task<Basket> UniteBasket(string anonBasketId, string userBasketId)
        {
            if (anonBasketId == null || userBasketId == null) return null;

            var anonBasket = await GetBasketAsync(anonBasketId);
            var userBasket = await GetBasketAsync(userBasketId);

            if (anonBasket == null || userBasket == null) return null;

            foreach (var item in anonBasket.Items)
            {
                var existItem = userBasket.Items.Find(x => x.ProductId == item.ProductId);

                if (existItem != null)
                {
                    existItem.Quantity += item.Quantity;
                }
                else
                {
                    userBasket.Items.Add(item);
                }
            }

            var basketRepository = _unitOfWork.Repository<Basket>();

            basketRepository.Delete(anonBasket);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            return userBasket;
        }

        public async Task<Basket> AddItemAsync(string buyerId, int productId, int quantity)
        {
            var spec = new BasketWithItemsSpecefications(buyerId);

            var basketRepository = _unitOfWork.Repository<Basket>();
            var productRepository = _unitOfWork.Repository<Product>();

            var product = await productRepository.GetByIdAsync(productId);

            if (product == null) return null;

            var basket = await basketRepository.GetEntityWithSpecAsync(spec);

            if (basket == null)
            {
                basket = await CreateBasket(buyerId);
            }

            basket.AddItem(productId, quantity);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            var returnBasket = await basketRepository.GetEntityWithSpecAsync(spec);

            return returnBasket;
        }

        public async Task<Basket> RemoveItemAsync(string buyerId, int productId, int quantity)
        {
            var spec = new BasketWithItemsSpecefications(buyerId);

            var basketRepository = _unitOfWork.Repository<Basket>();

            var basket = await basketRepository.GetEntityWithSpecAsync(spec);

            basket.RemoveItem(productId, quantity);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            var returnBasket = await basketRepository.GetEntityWithSpecAsync(spec);

            return returnBasket;
        }

        public async Task<Basket> DeleteBasketAsync(string buyerId)
        {
            var spec = new BasketWithItemsSpecefications(buyerId);

            var basketRepository = _unitOfWork.Repository<Basket>();

            var basket = await basketRepository.GetEntityWithSpecAsync(spec);

            basketRepository.Delete(basket);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            return basket;
        }
    }
}
