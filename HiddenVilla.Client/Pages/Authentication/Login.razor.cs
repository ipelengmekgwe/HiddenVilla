using HiddenVilla.Client.Service.IService;
using Microsoft.AspNetCore.Components;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HiddenVilla.Client.Pages.Authentication
{
    public partial class Login
    {
        [Inject]
        public IAuthenticationService _authenticationService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }


        private AuthenticationDto UserForAuthentication = new();
        public bool IsProcessing { get; set; } = false;
        public bool ShowAuthenticationErrors { get; set; }
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }


        private async Task AuthenticateUser()
        {
            ShowAuthenticationErrors = false;
            IsProcessing = true;

            var result = await _authenticationService.Login(UserForAuthentication);
            IsProcessing = false;

            if (result.IsAuthSuccessful)
            {
                var absoluteUri = new Uri(_navigationManager.Uri);
                var queryParam = HttpUtility.ParseQueryString(absoluteUri.Query);
                ReturnUrl = queryParam["returnUrl"];

                if (string.IsNullOrEmpty(ReturnUrl))
                {
                    _navigationManager.NavigateTo("/");
                }
                else
                {
                    _navigationManager.NavigateTo($"/{ReturnUrl}");
                }
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
                ShowAuthenticationErrors = true;
            }
        }
    }
}
