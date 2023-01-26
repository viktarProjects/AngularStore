using AngularStore.Core.Entities;
using AngularStore.Core.Entities.OrderAggregate;
using AngularStore.Core.Interfaces;
using AngularStore.WebAPI.Dto_s;
using AngularStore.WebAPI.Errors;
using AngularStore.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Web.Mvc;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using EmptyResult = Microsoft.AspNetCore.Mvc.EmptyResult;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace AngularStore.WebAPI.Controllers
{
    public class PaymentsController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string WhSecret = "whsec_2197daf23e1f551df71fbe51ae1573752f371352988c3dde63d51a7b0f39564a";

        public PaymentsController(IPaymentService paymentService,ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("{buyerId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string buyerId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(buyerId);

            if (basket == null) return BadRequest(new ApiResponse(400, "Problem with your basket"));

            return basket.MapBasketDto();
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WhSecret);

            PaymentIntent intent;
            Order order;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("payment Succeeded: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                    _logger.LogInformation("Order updated to payment received: ", order.Id);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Failed: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                    _logger.LogInformation("Payment Failed: ", order.Id);
                    break;
            }

            return new EmptyResult();
        }
    }
}
