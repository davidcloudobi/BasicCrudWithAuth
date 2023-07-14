using BusinessCore;
using BusinessCore.Interfaces;
using CoreObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                LogService.LogInfo("00", "PhoneController", "CreatePhone", "Response Details \r\n" + JsonConvert.SerializeObject(phone));
                var result = await _userServices.CreatePhone(phone);
                LogService.LogInfo("00", "PhoneController", "CreatePhone", "Response Details \r\n" + JsonConvert.SerializeObject(result));
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
                LogService.LogInfo("00", "PhoneController", "UpdatePhone", "Response Details \r\n" + JsonConvert.SerializeObject(phone));
                var result = await _userServices.UpdatePhone(phone);
                LogService.LogInfo("00", "PhoneController", "UpdatePhone", "Response Details \r\n" + JsonConvert.SerializeObject(result));
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
                LogService.LogInfo("00", "PhoneController", "GetPhone", "Response Details \r\n" + JsonConvert.SerializeObject(id));
                var result = await _userServices.GetPhone(id);
                LogService.LogInfo("00", "PhoneController", "GetPhone", "Response Details \r\n" + JsonConvert.SerializeObject(result));
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
                LogService.LogInfo("00", "PhoneController", "GetAllPhones", "Response Details \r\n" + JsonConvert.SerializeObject(result));
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
                LogService.LogInfo("00", "PhoneController", "SoftDeletePhone", "Response Details \r\n" + JsonConvert.SerializeObject(id));
                var result = await _userServices.SoftDeletePhone(id);
                LogService.LogInfo("00", "PhoneController", "SoftDeletePhone", "Response Details \r\n" + JsonConvert.SerializeObject(result));
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
                LogService.LogInfo("00", "PhoneController", "HardDeletePhone", "Response Details \r\n" + JsonConvert.SerializeObject(id));
                var result = await _userServices.HradDeletePhone(id);

                LogService.LogInfo("00", "PhoneController", "HardDeletePhone", "Response Details \r\n" + JsonConvert.SerializeObject(result));
                return Ok(result);
            }

            return BadRequest("Invalid details");
        }
    }
}
