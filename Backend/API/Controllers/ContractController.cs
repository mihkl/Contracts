using API.Data;
using API.Data.Repos;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static API.FileManipulation.FileManipulator;
using static API.Mappers.Mappers;

namespace API.Controllers;

[ApiController]
[Route("api")]
public class ContractController(IMemoryCache cache, ContractRepo repo) : ControllerBase
{
    private readonly ContractRepo _repo = repo;
    private readonly IMemoryCache _cache = cache;

    [HttpPost("upload")]
    public ActionResult<UploadFileResponse> UploadDocxFile(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }
        var contract = ParseDocxFile(file);

        var guid = Guid.NewGuid();
        _cache.Set(guid, contract, TimeSpan.FromMinutes(5));

        var response = new UploadFileResponse
        {
            Contract = ToContractDto(contract),
            Guid = guid
        };

        return Ok(response);
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveContract([FromBody] Guid guid)
    {
        if (guid == Guid.Empty)
        {
            return BadRequest("Invalid guid.");
        }
        if (!_cache.TryGetValue(guid, out Contract? contract))
        {
            return NotFound("File data not found or expired.");
        }
        var result = await _repo.SaveContractToDb(contract!);

        _cache.Remove(guid);

        return CreatedAtAction(nameof(GetContracts), new { id = result.Id }, ToContractDto(result));
    }

    [HttpGet("contracts")]
    public async Task<IActionResult> GetContracts()
    {
        var contracts = await _repo.GetContractsFromDb();
        var result = contracts.Select(ToContractDto).ToList();
        return Ok(result);
    }

    [HttpGet("contracts/{id}")]
    public async Task<IActionResult> GetContract(uint id)
    {
        var contract = await _repo.GetContractByIdFromDb(id);
        if (contract is null)
        {
            return NotFound("Contract not found.");
        }
        return Ok(ToContractDto(contract));
    }

    [HttpGet("contracts/{id}/file")]
    public async Task<IActionResult> GetContractFile(uint id)
    {
        var contract = await _repo.GetContractByIdFromDb(id);
        if (contract is null)
        {
            return NotFound("Contract not found.");
        }
        var contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        return File(contract.FileData, contentType, $"{contract.Name}");
    }

    [HttpPost("contracts/replace-fields")]
    public async Task<IActionResult> ReplaceDynamicFields([FromBody] UpdateFileRequest request)
    {
        if (request.Replacements is null || request.Replacements.Count == 0)
        {
            return BadRequest("No replacement fields provided.");
        }
        var contract = await _repo.GetContractByIdFromDb(request.ContractId);
        if (contract is null)
        {
            return NotFound("Contract not found.");
        }
        await _repo.ReplaceDynamicFields(request.Replacements, contract);

        return Ok("Dynamic fields updated successfully.");
    }

    [HttpDelete("contracts/{id}")]
    public async Task<IActionResult> DeleteContract(uint id)
    {
        var contract = await _repo.GetContractByIdFromDb(id);
        if (contract is null)
        {
            return NotFound("Contract not found.");
        }
        await _repo.DeleteContractFromDb(contract);
        return Ok("Contract deleted successfully.");
    }
}




