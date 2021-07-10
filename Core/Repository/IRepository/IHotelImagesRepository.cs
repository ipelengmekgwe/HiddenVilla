using DataAccess.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository.IRepository
{
    public interface IHotelImagesRepository
    {
        public Task<int> CreateHotelRoomImage(HotelRoomImageDto hotelRoomImageDto);
        public Task<int> DeleteHotelRoomImageById(int id);
        public Task<int> DeleteHotelRoomImageByRoomId(int roomId);
        public Task<int> DeleteHotelImageByImageUrl(string imageUrl);
        public Task<IEnumerable<HotelRoomImageDto>> GetHotelRoomImages(int roomId);
    }
}
