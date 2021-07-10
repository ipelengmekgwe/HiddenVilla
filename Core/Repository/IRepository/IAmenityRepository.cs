using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository.IRepository
{
    public interface IAmenityRepository
    {
        public Task<HotelAmenityDto> CreateHotelAmenity(HotelAmenityDto hotelAmenity);
        public Task<HotelAmenityDto> UpdateHotelAmenity(int amenityId, HotelAmenityDto hotelAmenity);
        public Task<int> DeleteHotelAmenity(int amenityId, string userId);
        public Task<IEnumerable<HotelAmenityDto>> GetAllHotelAmenity();
        public Task<HotelAmenityDto> GetHotelAmenity(int amenityId);
        public Task<HotelAmenityDto> IsSameNameAmenityAlreadyExists(string name);
    }
}
