using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/users")] //GET api/users
    public class UsersController : ControllerBase
    {
        private readonly DataContext context;
       public UsersController(DataContext context)
       {
        this.context = context;
       } 

       [HttpGet]
public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
    var users = await context.Users.ToListAsync(); //Users is die naam van die databasis
    return users;
}

[HttpGet("{id}")]
public async Task<ActionResult<AppUser>> GetUser(int id){
var user = await context.Users.FindAsync(id);
return user;
}    
    
    
    }
}