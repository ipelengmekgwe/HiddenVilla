using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla.Client.Service.IService
{
    public interface IHotelRoomService
    {
        public Task<IEnumerable<HotelRoomDto>> GetHotelRooms(string checkInDate, string checkOutDate);
        public Task<HotelRoomDto> GetHotelRoomDetails(int roomId, string checkInDate, string checkOutDate);
    }
}
