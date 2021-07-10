using HiddenVilla.Client.Service.IService;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla.Client.Pages.Authentication
{
    public partial class Logout
    {
        [Inject]
        public IAuthenticationService _authenticationService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await _authenticationService.Logout();
            _navigationManager.NavigateTo("/");
        }
    }
}
