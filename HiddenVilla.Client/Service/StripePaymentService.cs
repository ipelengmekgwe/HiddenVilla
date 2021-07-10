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
    public class StripePaymentService : IStripePaymentService
    {
        private readonly HttpClient _client;

        public StripePaymentService(HttpClient client)
        {
            _client = client;
        }

        public async Task<SuccessModel> CheckOut(StripePaymentDto model)
        {
            var content = JsonConvert.SerializeObject(model);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/stripepayment/create", bodyContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<SuccessModel>(responseContent);
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
