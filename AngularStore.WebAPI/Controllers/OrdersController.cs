using AngularStore.Core.Entities.OrderAggregate;
using AngularStore.Core.Interfaces;
using AngularStore.WebAPI.Dto_s;
using AngularStore.WebAPI.Errors;
using AngularStore.WebAPI.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AngularStore.WebAPI.Controllers
{
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var address = _mapper.Map<AddressDto, Address>(orderDto.Address);

            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

            if(order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var orders = await _orderService.GetOrdersAsync(email);

            return Ok(_mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUserById(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();

            var order = await _orderService.GetOrderByIdAsync(id,email);

            if (order == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Order,OrderToReturnDto>(order);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }
    }
}
