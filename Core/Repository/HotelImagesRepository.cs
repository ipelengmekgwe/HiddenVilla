using AutoMapper;
using Core.Repository.IRepository;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public class HotelImagesRepository : IHotelImagesRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public HotelImagesRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> CreateHotelRoomImage(HotelRoomImageDto hotelRoomImageDto)
        {
            var image = _mapper.Map<HotelRoomImageDto, HotelRoomImage>(hotelRoomImageDto);
            await _db.HotelRoomImages.AddAsync(image);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> DeleteHotelImageByImageUrl(string imageUrl)
        {
            var image = await _db.HotelRoomImages.FirstOrDefaultAsync(x => x.RoomImageUrl.ToLower() == imageUrl.ToLower());
            if (image == null) return 0;

            _db.HotelRoomImages.Remove(image);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> DeleteHotelRoomImageById(int id)
        {
            var image = await _db.HotelRoomImages.FindAsync(id);
            _db.HotelRoomImages.Remove(image);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> DeleteHotelRoomImageByRoomId(int roomId)
        {
            var images = await _db.HotelRoomImages.Where(x => x.RoomId == roomId).ToListAsync();
            _db.HotelRoomImages.RemoveRange(images);
            return await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<HotelRoomImageDto>> GetHotelRoomImages(int roomId)
        {
            return _mapper.Map<IEnumerable<HotelRoomImage>, IEnumerable<HotelRoomImageDto>>(
            await _db.HotelRoomImages.Where(x => x.RoomId == roomId).ToListAsync());
        }
    }
}
