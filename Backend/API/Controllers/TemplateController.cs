using API.Data.Repos;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static API.FileManipulation.FileManipulator;
using static API.Mappers.Mappers;

namespace API.Controllers;

[ApiController]
[Route("api")]
public class TemplateController(IMemoryCache cache, TemplateRepo repo) : ControllerBase
{
    private readonly TemplateRepo _repo = repo;
    private readonly IMemoryCache _cache = cache;

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
        else
        {
            return BadRequest(exception);
        }
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveTemplate([FromBody] SaveTemplateRequest request)
    {
        if (request.Guid == Guid.Empty)
        {
            return BadRequest("Invalid guid.");
        }
        if (!_cache.TryGetValue(request.Guid, out Template? template))
        {
            return NotFound("File data not found or expired.");
        }
        template!.Name = request.Name;
        var result = await _repo.Save(template!);

        _cache.Remove(request.Guid);

        return CreatedAtAction(nameof(GetTemplates), new { id = result.Id }, ToTemplateDto(result));
    }

    [HttpGet("templates")]
    public async Task<IActionResult> GetTemplates()
    {
        var templates = await _repo.GetAll();
        var result = templates.Select(ToTemplateDto).ToList();
        return Ok(result);
    }

    [HttpGet("templates/{id}")]
    public async Task<IActionResult> GetTemplate(uint id)
    {
        var template = await _repo.GetById(id);
        if (template is null)
        {
            return NotFound("Template not found.");
        }
        return Ok(ToTemplateDto(template));
    }

    [HttpGet("templates/{id}/file")]
    public async Task<IActionResult> GetTemplateFile(uint id)
    {
        var template = await _repo.GetById(id);
        if (template is null)
        {
            return NotFound("Template not found.");
        }
        var contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        return File(template.FileData, contentType, $"{template.Name}.docx");
    }

    [HttpDelete("templates/{id}")]
    public async Task<IActionResult> DeleteTemplate(uint id)
    {
        var template = await _repo.GetById(id);
        if (template is null)
        {
            return NotFound("Template not found.");
        }
        await _repo.Delete(template);
        return Ok("Template deleted successfully.");
    }

    [HttpGet("templates/testfile")]
    public async Task<IActionResult> GetTestFile()
    {
        string fileRelativePath = @"./testfiles/uploadtemplatetestfile.docx";
        string absolutePath = Path.GetFullPath(fileRelativePath);

        byte[] fileBytes = System.IO.File.ReadAllBytes(absolutePath);

        return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "uploadtemplatetestfile.docx");
    }
}




