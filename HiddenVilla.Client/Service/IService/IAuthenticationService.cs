using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla.Client.Service.IService
{
    public interface IAuthenticationService
    {
        Task<RegistrationResponseDto> RegisterUser(UserRequestDto userForRegistration);
        Task<AuthenticationResponseDto>Login(AuthenticationDto userFromAuthentiocation);
        Task Logout();
    }
}
