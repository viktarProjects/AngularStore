﻿using AngularStore.Core.Entities;
using AngularStore.Core.Interfaces;
using AngularStore.WebAPI.Dto_s;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AngularStore.WebAPI.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);

            return Ok(basket ?? new CustomerBasket(basketId));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var customerBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);

            var updatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);

            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string basketId)
        {
            await _basketRepository.DeleteBasketAsync(basketId);
        }
    }
}
