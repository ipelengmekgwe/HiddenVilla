using Common;
using DataAccess.Data;
using HiddenVilla.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HiddenVilla.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApiSettings _apiSettings;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<ApiSettings> options)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _apiSettings = options.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] UserRequestDto userRequestDto)
        {
            if (userRequestDto == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new ApplicationUser
            {
                UserName = userRequestDto.Email,
                Email = userRequestDto.Email,
                Name = userRequestDto.Name,
                PhoneNumber = userRequestDto.PhoneNo,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userRequestDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return BadRequest(new RegistrationResponseDto
                {
                    IsRegistrationSuccessful = false,
                    Errors = errors
                });
            }

            var roleResult = await _userManager.AddToRoleAsync(user, SD.RoleCustomer);
            if (!roleResult.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return BadRequest(new RegistrationResponseDto
                {
                    IsRegistrationSuccessful = false,
                    Errors = errors
                });
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] AuthenticationDto authenticationDto)
        {
            var result = await _signInManager.PasswordSignInAsync(authenticationDto.UserName, authenticationDto.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(authenticationDto.UserName);
                if (user == null)
                {
                    return Unauthorized(new AuthenticationResponseDto
                    {
                        IsAuthSuccessful = false,
                        ErrorMessage = "Invalid Authentication"
                    });
                }

                //everything is valid and login the user
                var signInCredentials = GetSigningCredentials();
                var claims = await GetClaims(user);

                var tokenOptions = new JwtSecurityToken(
                    issuer: _apiSettings.ValidIssuer,
                    audience: _apiSettings.ValidAudience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: signInCredentials);

                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return Ok(new AuthenticationResponseDto
                {
                    IsAuthSuccessful = true,
                    Token = token,
                    UserDto = new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNo = user.PhoneNumber
                    }
                });

            }
            else
            {
                return Unauthorized(new AuthenticationResponseDto 
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Invalid Authentication"
                });
            }
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSettings.SecurityKey));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            { 
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.Id)
            };

            var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(user.Email));

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
