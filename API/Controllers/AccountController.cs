using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {     
        private readonly DataContext _context;

        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService){
            _context = context; 
            _tokenService = tokenService;
        }

        //To Check if username already exists
        private async Task<bool> UserExists(string username){
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDto registerDto){

            if(await UserExists(registerDto.UserName)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser{
                UserName = registerDto.UserName.ToLower(),
                PassWordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PassWordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

    [HttpPost("login")]//root parameter of LOGIN 
    public async Task<ActionResult<UserDTO>> Login(LoginDto loginDto){ //call it Login,     //Public async task that will return an AppUser
    var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName ==loginDto.UserName);

    if(user == null) return Unauthorized("Invalid Username");

     using var hmac = new HMACSHA512(user.PassWordSalt);

     var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

     for(int i = 0; i< computedHash.Length; i++){
        if(computedHash[i] != user.PassWordHash[i]) return Unauthorized("Invalid Password");
     }

      return new UserDTO{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

    }
    

}
}