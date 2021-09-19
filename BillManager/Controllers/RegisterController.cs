using BillManager.Interfaces;
using BillManager.Models;
using BillManager.Models.ModelsDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillManager.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class RegisterController : Controller
    {      
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterController> _logger;
        private readonly IUsersService _usersService;

        public RegisterController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterController> logger,
            IUsersService usersService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._logger = logger;
            this._usersService = usersService;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseAfterAutDTO> Login([FromBody] UserDTO user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.Mail, user.Password, false, lockoutOnFailure: true);
            if(result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return _usersService.GetIdAndRoleForUserById(user.Mail);
            }
            else
            {
                _logger.LogInformation("User login failed.");
                return new ResponseAfterAutDTO
                {
                    Code = 400,
                    Message = "Login failed",
                    Status = "Failed"
                };
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ResponseAfterAutDTO> Register([FromBody] UserDTO user)
        {
            var newUser = new ApplicationUser
            {
                UserName = user.Name,
                Email = user.Mail,
                PhoneNumber = user.TelNumber.ToString(),
                IdPaid = false
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);
            if(result.Succeeded)
            {
                if(!await _roleManager.RoleExistsAsync("Admin"))
                {
                    var role = new IdentityRole("Admin");
                    var res = await _roleManager.CreateAsync(role);
                    if(res.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(newUser, "Admin");
                    }
                } 
                else
                {
                    if(!await _roleManager.RoleExistsAsync("User"))
                    {
                        var role = new IdentityRole("User");
                        var res = await _roleManager.CreateAsync(role);
                    }
                    await _userManager.AddToRoleAsync(newUser, "User");
                }
                _logger.LogInformation("User created a new account with password.");
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                _logger.LogInformation("User signed in.");
                return _usersService.GetIdAndRoleForUserById(user.Mail);
                
            }
            else
            {
                _logger.LogInformation("User registration failed.");
                return new ResponseAfterAutDTO
                {
                    Code = 400,
                    Message = "Login failed.",
                    Status = "Failed"
                };
            }
        }
    }
}
