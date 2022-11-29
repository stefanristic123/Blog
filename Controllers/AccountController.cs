using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Dto;
using Blog.Interfaces;
using Blog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        //private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController( ITokenService tokenService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            //_context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {
            if (await UserExists(register.UserName)) return BadRequest("Username is taken");
 
            var user = new User
            {
                UserName = register.UserName.ToLower(),
            };

            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if(!roleResult.Succeeded) return BadRequest(result.Errors);
         
            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }



        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(RegisterDto login)
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(x => x.UserName == login.UserName.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            var result = await _userManager
                .CheckPasswordAsync(user, login.Password);

            if (!result) return Unauthorized("Invalid password");

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}

