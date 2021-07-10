using HiddenVilla.Client.Service.IService;
using Microsoft.AspNetCore.Components;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla.Client.Pages.Authentication
{
    public partial class Register
    {
        [Inject]
        public IAuthenticationService _authenticationService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }


        private UserRequestDto UserForRegistration = new();
        public bool IsProcessing { get; set; } = false;
        public bool ShowRegistrationErrors { get; set; }
        public IEnumerable<string> Errors { get; set; }


        private async Task RegisterUser()
        {
            ShowRegistrationErrors = false;
            IsProcessing = true;

            var result = await _authenticationService.RegisterUser(UserForRegistration);
            IsProcessing = false;

            if (result.IsRegistrationSuccessful)
            {

                _navigationManager.NavigateTo("/login");
            }
            else
            {
                Errors = result.Errors;
                ShowRegistrationErrors = true;
            }
        }
    }
}
