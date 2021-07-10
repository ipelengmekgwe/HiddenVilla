using Blazored.LocalStorage;
using Common;
using HiddenVilla.Client.Service.IService;
using Microsoft.AspNetCore.Components.Authorization;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HiddenVilla.Client.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthenticationService(HttpClient client, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _client = client;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<AuthenticationResponseDto> Login(AuthenticationDto userFromAuthentiocation)
        {
            var content = JsonConvert.SerializeObject(userFromAuthentiocation);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/account/signin", bodyContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AuthenticationResponseDto>(responseContent);

            if (response.IsSuccessStatusCode)
            {
                await _localStorage.SetItemAsync(SD.LocalToken, result.Token);
                await _localStorage.SetItemAsync(SD.LocalUserDetails, result.UserDto);
                ((AuthStateProvider)_authStateProvider).NotifyUserLoggedIn(result.Token);

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

                return new AuthenticationResponseDto { IsAuthSuccessful = true };
            }
            else
            {
                return result;
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync(SD.LocalToken);
            await _localStorage.RemoveItemAsync(SD.LocalUserDetails);
            ((AuthStateProvider)_authStateProvider).NotifyUserLoggedOut();

            _client.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<RegistrationResponseDto> RegisterUser(UserRequestDto userForRegistration)
        {
            var content = JsonConvert.SerializeObject(userForRegistration);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/account/signup", bodyContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RegistrationResponseDto>(responseContent);

            if (response.IsSuccessStatusCode)
            {
                return new RegistrationResponseDto {  IsRegistrationSuccessful = true };
            }
            else
            {
                return result;
            }
        }
    }
}
