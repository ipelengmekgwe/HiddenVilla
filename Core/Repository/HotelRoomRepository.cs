using AutoMapper;
using Core.Repository.IRepository;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public class HotelRoomRepository : IHotelRoomRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public HotelRoomRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<HotelRoomDto> CreateHotelRoom(HotelRoomDto hotelRoomDto)
        {
            HotelRoom hotelRoom = _mapper.Map<HotelRoomDto, HotelRoom>(hotelRoomDto);
            hotelRoom.CreatedDate = DateTime.Now;
            hotelRoom.CreatedBy = string.Empty;
            var addedHotelRoom = await _db.HotelRooms.AddAsync(hotelRoom);
            await _db.SaveChangesAsync();

            return _mapper.Map<HotelRoom, HotelRoomDto>(addedHotelRoom.Entity);
        }

        public async Task<int> DeleteHotelRoom(int id)
        {
            var roomDetails = await _db.HotelRooms.FindAsync(id);
            if (roomDetails != null)
            {
                var allImages = await _db.HotelRoomImages.Where(x => x.RoomId == id).ToListAsync();

                foreach (var image in allImages)
                {
                    if (File.Exists(image.RoomImageUrl))
                    {
                        File.Delete(image.RoomImageUrl);
                    }
                }

                _db.HotelRoomImages.RemoveRange(allImages);
                _db.HotelRooms.Remove(roomDetails);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<HotelRoomDto>> GetAllHotelRooms(string checkInDateStr = null, string checkOutDateStr = null)
        {
            try
            {
                IEnumerable<HotelRoomDto> hotelRoomDtos = _mapper.Map<IEnumerable<HotelRoom>, IEnumerable<HotelRoomDto>>(
                     _db.HotelRooms.Include(x => x.HotelRoomImages));

                if (!string.IsNullOrEmpty(checkInDateStr) && !string.IsNullOrEmpty(checkOutDateStr))
                {
                    foreach (var hotelRoom in hotelRoomDtos)
                    {
                        hotelRoom.IsBooked = await IsRoomBooked(hotelRoom.Id, checkInDateStr, checkOutDateStr);
                    }
                }

                return hotelRoomDtos;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<HotelRoomDto> GetHotelRoom(int id, string checkInDateStr = null, string checkOutDateStr = null)
        {
            try
            {
                var hotelRoom = _mapper.Map<HotelRoom, HotelRoomDto>(
                    await _db.HotelRooms.Include(x => x.HotelRoomImages).FirstOrDefaultAsync(x => x.Id == id));

                if (!string.IsNullOrEmpty(checkInDateStr) && !string.IsNullOrEmpty(checkOutDateStr))
                {
                    hotelRoom.IsBooked = await IsRoomBooked(id, checkInDateStr, checkOutDateStr);
                }

                return hotelRoom;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> IsRoomBooked(int roomId, string checkInDateStr, string checkOutDateStr)
        {
            try
            {
                if (!string.IsNullOrEmpty(checkInDateStr) && !string.IsNullOrEmpty(checkOutDateStr))
                {
                    DateTime checkInDate = DateTime.ParseExact(checkInDateStr, "yyyy/MM/dd", null);
                    DateTime checkOutDate = DateTime.ParseExact(checkOutDateStr, "yyyy/MM/dd", null);

                    var existingBooking = await _db.RoomOrderDetials.Where(x => x.RoomId == roomId && x.IsPaymentSuccessful &&
                    ((checkInDate < x.CheckOutDate && checkInDate >= x.CheckInDate) || (checkOutDate.Date > x.CheckInDate.Date && checkInDate.Date <= x.CheckInDate.Date)))
                        .FirstOrDefaultAsync();

                    if (existingBooking != null)
                    {
                        return true;
                    }

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HotelRoomDto> IsRoomUnique(string name, int id = 0)
        {
            try
            {
                if (id == 0)
                {
                    var hotelRoom = _mapper.Map<HotelRoom, HotelRoomDto>(
                    await _db.HotelRooms.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()));

                    return hotelRoom;
                }
                else
                {
                    var hotelRoom = _mapper.Map<HotelRoom, HotelRoomDto>(
                    await _db.HotelRooms.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower() && x.Id != id));

                    return hotelRoom;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<HotelRoomDto> UpdateHotelRoom(int id, HotelRoomDto hotelRoomDto)
        {
            try 
            {
                if (id == hotelRoomDto.Id)
                {
                    var roomDetails = await _db.HotelRooms.FindAsync(id);
                    var hotelRoom = _mapper.Map<HotelRoomDto, HotelRoom>(hotelRoomDto, roomDetails);
                    hotelRoom.UpdatedBy = string.Empty;
                    hotelRoom.UpdatedDate = DateTime.Now;

                    var updatedRoom = _db.HotelRooms.Update(hotelRoom);
                    await _db.SaveChangesAsync();

                    return _mapper.Map<HotelRoom, HotelRoomDto>(updatedRoom.Entity);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
