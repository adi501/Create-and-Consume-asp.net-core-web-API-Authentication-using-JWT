using ASP_Dot_NETCore_Web_API_Authentication_Using_JWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Dot_NETCore_Web_API_Authentication_Using_JWT
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IJwtAuth jwtAuth;

        private readonly List<User> lstUser = new List<User>()
        {
            new User{Id=1, Name="Adi" },
            new User {Id=2, Name="Pavan" },
            new User{Id=3, Name="Madhan"},
            new User{Id=4, Name="Yagnesh"}
        };
        public UsersController(IJwtAuth jwtAuth)
        {
            this.jwtAuth = jwtAuth;
        }
        // GET: api/<UsersController>
       // [AllowAnonymous]
        [HttpGet]
        public object AllUsers()
        {
            return  JsonConvert.SerializeObject(lstUser);
           // return JsonResult(lstUser);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public User UserByid(int id)
        {
            return lstUser.Find(x => x.Id == id);
        }

        [AllowAnonymous]
        // POST api/<UsersController>
        [HttpPost("authentication")]
        public object Authentication([FromBody] LoginCredential userCredential)
        {
            var token = jwtAuth.Authentication(userCredential.UserName, userCredential.Password);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }

    }
}
