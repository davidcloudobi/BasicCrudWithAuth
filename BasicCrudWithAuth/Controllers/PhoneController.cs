using BusinessCore.Interfaces;
using CoreObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicCrudWithAuth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhoneController : ControllerBase
    {
        private IUserService _userServices { get; set; }

        public PhoneController(IUserService userService)
        {
            _userServices = userService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]

        public async Task<IActionResult> CreatePhone(AddPhoneDto phone)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.CreatePhone(phone);

                return Ok(result);
            }

            return BadRequest("Invalid details");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]

        public async Task<IActionResult> UpdatePhone(UpdatePhoneDto phone)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.UpdatePhone(phone);

                return Ok(result);
            }

            return BadRequest("Invalid details");
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]

        public async Task<IActionResult> GetPhone([FromQuery]int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.GetPhone(id);

                return Ok(result);
            }

            return BadRequest("Invalid details");
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]

        public async Task<IActionResult> GetAllPhones()
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.GetPhones();

                return Ok(result);
            }

            return BadRequest("Invalid details");
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]

        public async Task<IActionResult> SoftDeletePhone([FromQuery] int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.SoftDeletePhone(id);

                return Ok(result);
            }

            return BadRequest("Invalid details");
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]

        public async Task<IActionResult> HardDeletePhone([FromQuery] int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.HradDeletePhone(id);

                return Ok(result);
            }

            return BadRequest("Invalid details");
        }
    }
}
