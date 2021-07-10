using Common;
using Core.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla.Api.Controllers
{
    [Route("api/[controller]")]
    public class HotelRoomController : Controller
    {
        private readonly IHotelRoomRepository _hotelRoomRepository;

        public HotelRoomController(IHotelRoomRepository hotelRoomRepository)
        {
            _hotelRoomRepository = hotelRoomRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetHotelRooms(string checkInDate = null, string checkOutDate = null)
        {
            if (string.IsNullOrEmpty(checkInDate) || string.IsNullOrEmpty(checkOutDate))
            {
                return BadRequest(new ErrorModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "All parameters needs to be supplied"
                });
            }

            if (!DateTime.TryParseExact(checkInDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return BadRequest(new ErrorModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid check-in date format, valid format will be yyyy/MM/dd"
                });
            }

            if (!DateTime.TryParseExact(checkOutDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return BadRequest(new ErrorModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid check-out date format, valid format will be yyyy/MM/dd"
                });
            }

            var allRooms = await _hotelRoomRepository.GetAllHotelRooms(checkInDate, checkOutDate);
            return Ok(allRooms);
        }

        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetHotelRoom(int? roomId, string checkInDate = null, string checkOutDate = null)
        {
            if (roomId == null) return BadRequest(new ErrorModel()
            {
                ErrorMessage = "Invalid Room Id",
                StatusCode = StatusCodes.Status400BadRequest
            });

            if (string.IsNullOrEmpty(checkInDate) || string.IsNullOrEmpty(checkOutDate))
            {
                return BadRequest(new ErrorModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "All parameters needs to be supplied"
                });
            }

            if (!DateTime.TryParseExact(checkInDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return BadRequest(new ErrorModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid check-in date format, valid format will be yyyy/MM/dd"
                });
            }

            if (!DateTime.TryParseExact(checkOutDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return BadRequest(new ErrorModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid check-out date format, valid format will be yyyy/MM/dd"
                });
            }

            var roomDetails = await _hotelRoomRepository.GetHotelRoom(roomId.Value, checkInDate, checkOutDate);
            if (roomDetails == null) return BadRequest(new ErrorModel()
            {
                ErrorMessage = "Invalid Room Id",
                StatusCode = StatusCodes.Status404NotFound
            });

            return Ok(roomDetails);
        }
    }
}
