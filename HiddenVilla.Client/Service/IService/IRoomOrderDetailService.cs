using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla.Client.Service.IService
{
    public interface IRoomOrderDetailService
    {
        public Task<RoomOrderDetailDto> SaveRoomOrderDetail(RoomOrderDetailDto detail);
        public Task<RoomOrderDetailDto> MarkPaymentSuccessful(RoomOrderDetailDto detail);
    }
}
