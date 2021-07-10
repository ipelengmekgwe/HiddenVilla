using Core.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Models;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoomOrderController : Controller
    {
        private readonly IRoomOrderDetailRepository _repository;
        private readonly IEmailSender _emailSender;

        public RoomOrderController(IRoomOrderDetailRepository repository, IEmailSender emailSender)
        {
            _repository = repository;
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomOrderDetailDto detail)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.Create(detail);
                return Ok(result);
            }
            else
            {
                return BadRequest(new ErrorModel
                {
                    ErrorMessage = "Error while creating room details / Booking",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PaymentSuccessful([FromBody] RoomOrderDetailDto detail)
        {
            var service = new SessionService();
            var sessionDetails = service.Get(detail.StripeSessionId);
            if (sessionDetails.PaymentStatus == "paid")
            {
                var result = await _repository.MarkPaymentSuccessful(detail.Id);
                if (result == null)
                {
                    return BadRequest(new ErrorModel
                    {
                        ErrorMessage = "Cannot mark payment as successful",
                        StatusCode = StatusCodes.Status400BadRequest
                    });
                }

                await _emailSender.SendEmailAsync($"{detail.Email};{detail.Name}", "Booking Confirmed - Hidden Villa",
                    $"Your booking has been confirmed at Hidden Villas with Order ID: {detail.Id}");

                return Ok(result);
            }
            else
            {
                return BadRequest(new ErrorModel
                {
                    ErrorMessage = "Cannot mark payment as successful",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

        }
    }
}
