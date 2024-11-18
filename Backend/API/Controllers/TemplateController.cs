using API.Data;
using API.Data.Repos;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static API.FileManipulation.FileManipulator;
using static API.Mappers.Mappers;

namespace API.Controllers;

[ApiController]
[Authorize]
public class TemplateController(IMemoryCache cache, TemplateRepo repo, UserManager<User> userManager) : ControllerBase
{
    private readonly TemplateRepo _repo = repo;
    private readonly IMemoryCache _cache = cache;
    private readonly UserManager<User> _userManager = userManager;

    [Authorize]
    [HttpPost("upload")]
    public ActionResult<UploadFileResponse> UploadDocxFile(IFormFile file)
    {
        var template = ParseDocxFile(file, out var exception);

        if (template is not null)
        {
            var guid = Guid.NewGuid();
            _cache.Set(guid, template, TimeSpan.FromMinutes(5));

            var response = new UploadFileResponse
            {
                Template = ToTemplateDto(template),
                Guid = guid,
            };
            return Ok(response);
        }
        return BadRequest(exception);
    }

    [Authorize]
    [HttpPost("save")]
    public async Task<IActionResult> SaveTemplate([FromBody] SaveTemplateRequest request)
    {
        var userId = _userManager.GetUserId(User);
        if (userId is null)
        {
            return BadRequest("Invalid user.");
        }
        if (request.Guid == Guid.Empty)
        {
            return BadRequest("Invalid guid.");
        }
        if (!_cache.TryGetValue(request.Guid, out Template? template))
        {
            return NotFound("File data not found or expired.");
        }
        template!.Name = request.Name;
        var result = await _repo.Save(template!, userId);

        _cache.Remove(request.Guid);

        return CreatedAtAction(nameof(GetTemplates), new { id = result.Id }, ToTemplateDto(result));
    }

    [Authorize]
    [HttpGet("templates")]
    public async Task<IActionResult> GetTemplates()
    {
        var userId = _userManager.GetUserId(User);
        var templates = await _repo.GetAll(userId);
        var result = templates.Select(ToTemplateDto).ToList();
        return Ok(result);
    }

    [Authorize]
    [HttpGet("templates/{id}")]
    public async Task<IActionResult> GetTemplate(uint id)
    {
        var userId = _userManager.GetUserId(User);
        var template = await _repo.GetById(id, userId);
        if (template is null)
        {
            return NotFound("Template not found.");
        }
        return Ok(ToTemplateDto(template));
    }

    [Authorize]
    [HttpGet("templates/{id}/file")]
    public async Task<IActionResult> GetTemplateFile(uint id)
    {
        var userId = _userManager.GetUserId(User);
        var template = await _repo.GetById(id, userId);
        if (template is null)
        {
            return NotFound("Template not found.");
        }
        var contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        return File(template.FileData, contentType, $"{template.Name}.docx");
    }

    [Authorize]
    [HttpDelete("templates/{id}")]
    public async Task<IActionResult> DeleteTemplate(uint id)
    {
        var userId = _userManager.GetUserId(User);
        var template = await _repo.GetById(id, userId);
        if (template is null)
        {
            return NotFound("Template not found.");
        }
        await _repo.Delete(template);
        return Ok("Template deleted successfully.");
    }
}




