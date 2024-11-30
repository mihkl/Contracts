using API.Data;
using API.Data.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
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
        public async Task<IActionResult> AddUpdateSmtpSettings()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return Unauthorized();

            var smtpSettings = await _settingsRepo.GetSmtpSettings(userId);

            return Ok(smtpSettings);
        }
    }
}