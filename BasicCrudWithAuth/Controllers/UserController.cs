using BusinessCore.Interfaces;
using BusinessCore.Services;
using CoreObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicCrudWithAuth.Controllers
{


        [Route("api/[controller]")]
        [ApiController]
        public class UserController : ControllerBase
        {
            private readonly IUserService _userServices;


            public UserController(IUserService userServices)
            {
                _userServices = userServices;

            }


            [HttpPost("login")]
            public async Task<IActionResult> Login(UserLoginDTO user)
            {
                if (ModelState.IsValid)
                {
                    var result = await _userServices.Login(user);

                    return Ok(result);
                }

                return BadRequest("Invalid details");
            }





            [HttpPost("register")]
            public async Task<IActionResult> Register(UserRegDTO user)
            {
                if (ModelState.IsValid)
                {
                    var result = await _userServices.Register(user);

                    return Ok(result);
                }

                return BadRequest("Invalid details");
            }



            [HttpGet("get-admin-role-name")]
            public async Task<IActionResult> Roles()
            {
            return Ok(new
            {
                name = "Admin"
            });
               
            }

            [HttpGet("logout")]
            [Authorize(AuthenticationSchemes = "Bearer")]
            public async Task<IActionResult> LogOut()
            {
                    var user = await _userServices.LogOut();
                    if (user == null) return BadRequest();
                    return  user.Status ? Ok(user) : NoContent();
            }
     
        }
    
}
