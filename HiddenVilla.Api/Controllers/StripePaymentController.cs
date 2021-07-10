using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StripePaymentController : Controller
    {
        private readonly IConfiguration _configuration;

        public StripePaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Create(StripePaymentDto payment)
        {
            try
            {
                var domain = string.Empty;
#if DEBUG
                domain = _configuration.GetValue<string>("HiddenVillaClientUrlLocal");
#else
                domain = _configuration.GetValue<string>("HiddenVillaClientUrl");
#endif

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = payment.Amount, //convert to cents
                                Currency = "zar",
                                ProductData = new SessionLineItemPriceDataProductDataOptions { Name = payment.ProductName }
                            },
                            Quantity = 1
                        }
                    },
                    Mode = "payment",
                    SuccessUrl = domain + "/success-payment?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = domain + payment.ReturnUrl
                };

                var service = new SessionService();
                Session session = await service.CreateAsync(options);

                return Ok(new SuccessModel
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = session.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel
                {
                    ErrorMessage = ex.Message,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
        }
    }
}
