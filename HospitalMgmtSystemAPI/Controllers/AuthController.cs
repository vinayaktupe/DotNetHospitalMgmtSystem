using HospitalMgmtSystem.DAL.Data.Model;
using HospitalMgmtSystem.Services.Services;
using HospitalMgmtSystem.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

        public AuthController(IConfiguration config, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IUserService userService)
        {
            this._config = config;
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginReceived)
        {
            IActionResult response = Unauthorized();
            var user = await AuthenticateUser(loginReceived);

            if (user != null)
            {
                var tokenString = GenerateJWT(user);
                response = Ok(new { token = tokenString });
            }

            return response;

        }
        protected async Task<ApplicationUser> AuthenticateUser(LoginModel credentials)
        {
            var result = await _signInManager.PasswordSignInAsync(credentials.Email, credentials.Password, credentials.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return await _userManager.FindByEmailAsync(credentials.Email);
            }

            return null;

        }

        protected string GenerateJWT(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Role", user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
               _config["Jwt:Issuer"],
               claims,
               expires: DateTime.Now.AddMinutes(120),
               signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
