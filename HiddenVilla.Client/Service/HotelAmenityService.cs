using HiddenVilla.Client.Service.IService;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HiddenVilla.Client.Service
{
    public class HotelAmenityService : IHotelAmenityService
    {

        private readonly HttpClient _client;

        public HotelAmenityService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<HotelAmenityDto>> GetHotelAmenities()
        {
            var response = await _client.GetAsync($"api/hotelamenity");
            var content = await response.Content.ReadAsStringAsync();
            var amenities = JsonConvert.DeserializeObject<IEnumerable<HotelAmenityDto>>(content);
            return amenities;
        }
    }
}
