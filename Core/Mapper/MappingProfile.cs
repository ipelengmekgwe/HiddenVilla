using AutoMapper;
using DataAccess.Data;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HotelRoomDto, HotelRoom>();
            CreateMap<HotelRoom, HotelRoomDto>();
            CreateMap<HotelAmenity, HotelAmenityDto>().ReverseMap();

            CreateMap<HotelRoomImageDto, HotelRoomImage>().ReverseMap();

            CreateMap<RoomOrderDetial, RoomOrderDetailDto>().ForMember(x => x.HotelRoomDto, opt => opt.MapFrom(c => c.HotelRoom));
            CreateMap<RoomOrderDetailDto, RoomOrderDetial>();
        }
    }
}
