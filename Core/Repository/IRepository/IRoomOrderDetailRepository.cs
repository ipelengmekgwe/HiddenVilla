using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository.IRepository
{
    public interface IRoomOrderDetailRepository
    {
        public Task<RoomOrderDetailDto> Create(RoomOrderDetailDto detail);
        public Task<RoomOrderDetailDto> MarkPaymentSuccessful(int id);
        public Task<RoomOrderDetailDto> GetRoomOrderDetail(int roomOrderId);
        public Task<IEnumerable<RoomOrderDetailDto>> GetRoomOrderDetails();
        public Task<bool> UpdateOrderStatus(int roomOrderId, string status);
    }
}
