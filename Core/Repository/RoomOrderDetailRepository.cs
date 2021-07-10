using AutoMapper;
using Common;
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
    public class RoomOrderDetailRepository : IRoomOrderDetailRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public RoomOrderDetailRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<RoomOrderDetailDto> Create(RoomOrderDetailDto detail)
        {
            try
            {
                detail.CheckInDate = detail.CheckInDate.Date;
                detail.CheckOutDate = detail.CheckOutDate.Date;

                var roomOrder = _mapper.Map<RoomOrderDetailDto, RoomOrderDetial>(detail);
                roomOrder.Status = SD.StatusPending;

                var result = await _db.RoomOrderDetials.AddAsync(roomOrder);
                await _db.SaveChangesAsync();

                return _mapper.Map<RoomOrderDetial, RoomOrderDetailDto>(result.Entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<RoomOrderDetailDto> GetRoomOrderDetail(int roomOrderId)
        {
            try
            {
                RoomOrderDetial roomOrder = await _db.RoomOrderDetials
                    .Include(x => x.HotelRoom)
                    .ThenInclude(x => x.HotelRoomImages)
                    .FirstOrDefaultAsync(x => x.Id == roomOrderId);

                RoomOrderDetailDto roomOrderDetailDto = _mapper.Map<RoomOrderDetial, RoomOrderDetailDto>(roomOrder);
                roomOrderDetailDto.HotelRoomDto.TotalDays = roomOrderDetailDto.CheckOutDate.Subtract(roomOrderDetailDto.CheckInDate).Days;
                roomOrderDetailDto.HotelRoomDto.TotalAmount = roomOrderDetailDto.HotelRoomDto.TotalDays * roomOrderDetailDto.HotelRoomDto.RegularRate;

                return roomOrderDetailDto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<RoomOrderDetailDto>> GetRoomOrderDetails()
        {
            try
            {
                IEnumerable<RoomOrderDetailDto> roomOrders = _mapper.Map<IEnumerable<RoomOrderDetial>, IEnumerable<RoomOrderDetailDto>>
                    (_db.RoomOrderDetials.Include(x => x.HotelRoom));

                return roomOrders;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<RoomOrderDetailDto> MarkPaymentSuccessful(int id)
        {
            var data = await _db.RoomOrderDetials.FindAsync(id);
            if (data == null)
            {
                return null;
            }

            if (!data.IsPaymentSuccessful)
            {
                data.IsPaymentSuccessful = true;
                data.Status = SD.StatusBooked;

                var markPaymentSuccessful = _db.RoomOrderDetials.Update(data);
                await _db.SaveChangesAsync();

                return _mapper.Map<RoomOrderDetial, RoomOrderDetailDto>(markPaymentSuccessful.Entity);
            }

            return new RoomOrderDetailDto();

        }

        public async Task<bool> UpdateOrderStatus(int roomOrderId, string status)
        {
            try
            {
                var roomOrder = await _db.RoomOrderDetials.FirstOrDefaultAsync(x => x.Id == roomOrderId);

                if (roomOrder == null)
                {
                    return false;
                }

                roomOrder.Status = status;
                if (status == SD.StatusCheckedIn)
                {
                    roomOrder.ActualCheckInDate = DateTime.Now;
                }

                if (status == SD.StatusCheckedOut)
                {
                    roomOrder.ActualCheckOutDate = DateTime.Now;
                }

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
