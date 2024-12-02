using API.Data;
using API.Data.Repos;
using API.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("settings")]
    public class SettingsController(UserManager<User> userManager, ISettingsRepo settingsRepo) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly ISettingsRepo _settingsRepo = settingsRepo;

        [Authorize]
        [HttpGet("smtp")]
        public async Task<IActionResult> GetSmtpSettings()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return Unauthorized();

            var smtpSettings = await _settingsRepo.GetSmtpSettings(userId);

            return Ok(smtpSettings);
        }

        [Authorize]
        [HttpPost("smtp")]
        public async Task<IActionResult> AddUpdateSmtpSettings([FromBody] AddUpdateSmtpSettings smtpSettingsDto)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return Unauthorized();

            await _settingsRepo.AddUpdateSmtpSettings(userId, smtpSettingsDto);

            return Ok();
        }

        [Authorize]
        [HttpDelete("smtp")]
        public async Task<IActionResult> DeleteSmtpSettings()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return Unauthorized();

            await _settingsRepo.DeleteSmtpSettings(userId);

            return Ok();
        }
    }
}