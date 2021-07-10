using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository.IRepository
{
    public interface IHotelRoomRepository
    {
        public Task<HotelRoomDto> CreateHotelRoom(HotelRoomDto hotelRoom);
        public Task<HotelRoomDto> UpdateHotelRoom(int id, HotelRoomDto hotelRoom);
        public Task<HotelRoomDto> GetHotelRoom(int id, string checkInDate = null, string checkOutDate = null);
        public Task<IEnumerable<HotelRoomDto>> GetAllHotelRooms(string checkInDate = null, string checkOutDate = null);
        public Task<HotelRoomDto> IsRoomUnique(string name, int id = 0);
        public Task<int> DeleteHotelRoom(int id);
        public Task<bool> IsRoomBooked(int roomId, string checkInDate, string checkOutDate);

        
    }
}
