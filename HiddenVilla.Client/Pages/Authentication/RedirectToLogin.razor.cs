using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla.Client.Pages.Authentication
{
    public partial class RedirectToLogin
    {
        [Inject]
        public NavigationManager _navigationManager { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> _authenticationState { get; set; }

        public bool NotAuthorized { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            var authState = await _authenticationState;

            if (authState?.User?.Identity is null || !authState.User.Identity.IsAuthenticated)
            {
                var returnUrl = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);

                if (string.IsNullOrEmpty(returnUrl))
                {
                    _navigationManager.NavigateTo("login", true);
                }
                else
                {
                    _navigationManager.NavigateTo($"login?returnUrl={returnUrl}", true);
                }
            }
            else
            {
                NotAuthorized = true;
            }
            
        }
    }
}
