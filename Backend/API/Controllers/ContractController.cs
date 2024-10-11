using API.Data.Repos;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using static API.Mappers.Mappers;

namespace API.Controllers;

[ApiController]
[Route("api")]
public class ContractController(ContractRepo crepo, TemplateRepo trepo, IHMACService hmacService) : ControllerBase
{
    private readonly ContractRepo _crepo = crepo;
    private readonly TemplateRepo _trepo = trepo;
    private readonly IHMACService _hmacService = hmacService;


    [HttpGet("contracts")]
    public async Task<IActionResult> GetContracts()
    {
        var contracts = await _crepo.GetAll();
        var result = contracts.Select(ToContractDto).ToList();
        return Ok(result);
    }

    [HttpGet("contracts/{id}")]
    public async Task<IActionResult> GetContract(uint id)
    {
        var contract = await _crepo.GetById(id);
        if (contract is null)
        {
            return NotFound("Contract not found.");
        }
        return Ok(ToContractDto(contract));
    }

    [HttpGet("contracts/{id}/file")]
    public async Task<IActionResult> GetContractFile(uint id)
    {
        var contract = await _crepo.GetById(id);
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
        var contract = await _crepo.GetById(request.ContractId);
        if (contract is null)
        {
            return NotFound("Contract not found.");
        }
        await _crepo.ReplaceDynamicFields(request.Replacements, contract);

        return Ok("Dynamic fields updated successfully.");
    }

    [HttpDelete("contracts/{id}")]
    public async Task<IActionResult> DeleteContract(uint id)
    {
        var contract = await _crepo.GetById(id);
        if (contract is null)
        {
            return NotFound("Contract not found.");
        }
        await _crepo.Delete(contract);
        return Ok("Contract deleted successfully.");
    }

    [HttpPost("contracts/{id}/url")]
    public async Task<IActionResult> GetContractLink(uint id, [FromBody] GenerateContractLinkRequest request)
    {
        var template = await _trepo.GetById(id);
        if (template is null)
        {
            return NotFound("Template not found.");
        }

        var contract = new Contract
        {
            Name = request.Name,
            FileData = template.FileData,
            Fields = template.Fields.Select(ToContractDynamicField).ToList()
        };

        var result = await _crepo.Save(contract);

        return Ok(new { url = $"contracts/{id}?signature={_hmacService.GenerateSignature(DateTime.Now, DateTime.Now, "2")}" });

    }
}




