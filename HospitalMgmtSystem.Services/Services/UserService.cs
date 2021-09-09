using HospitalMgmtSystem.DAL.Data;
using HospitalMgmtSystem.DAL.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgmtSystem.Services.Services
{
    public interface IUserService
    {
        public Task<bool> CreateUser(ApplicationUser user, string role);
    }
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public UserService(UserManager<ApplicationUser> userManager, ILogger<ApplicationUser> logger)
        {
            this._userManager = userManager;
            this._logger = logger;
        }
        public async Task<bool> CreateUser(ApplicationUser user, string role)
        {
            try
            {
                var result = await _userManager.CreateAsync(user, "User@123");
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, role);

                    return true;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"Error: {error.Code} - {error.Description}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Source} - {ex.Message}");
            }
            return false;
        }
    }
}
