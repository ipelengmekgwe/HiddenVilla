using HiddenVilla.Client.Service.IService;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HiddenVilla.Client.Service
{
    public class RoomOrderDetailService : IRoomOrderDetailService
    {
        private readonly HttpClient _client;

        public RoomOrderDetailService(HttpClient client)
        {
            _client = client;
        }

        public async Task<RoomOrderDetailDto> MarkPaymentSuccessful(RoomOrderDetailDto detail)
        {
            var content = JsonConvert.SerializeObject(detail);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/roomorder/paymentsuccessful", bodyContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RoomOrderDetailDto>(responseContent);
                return result;
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ErrorModel>(responseContent);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<RoomOrderDetailDto> SaveRoomOrderDetail(RoomOrderDetailDto detail)
        {
            var content = JsonConvert.SerializeObject(detail);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/roomorder/create", bodyContent);
            //string res = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RoomOrderDetailDto>(responseContent);
                return result;
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var errorModel = JsonConvert.DeserializeObject<ErrorModel>(responseContent);
                throw new Exception(errorModel.ErrorMessage);
            }
        }
    }
}
