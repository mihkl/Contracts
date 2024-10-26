using API.Data.Repos;
using API.FileConversion;
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
    public async Task<IActionResult> GetContract(uint id, [FromQuery] string signature, [FromQuery] DateTime validFrom, [FromQuery] DateTime validUntil)
    {
        if (!_hmacService.IsValidSignature(validFrom, validUntil, id.ToString(), signature)) return BadRequest("Invalid signature");

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
        return File(contract.FileData, contentType, $"{contract.Name}.docx");
    }

    [HttpPost("contracts/{id}/replace-fields")]
    public async Task<IActionResult> ReplaceDynamicFields([FromRoute] uint id, [FromBody] UpdateFileRequest request)
    {
        if (request.Replacements is null || request.Replacements.Count == 0)
        {
            return BadRequest("No replacement fields provided.");
        }
        var contract = await _crepo.GetById(id);
        if (contract is null)
        {
            return NotFound("Contract not found.");
        }
        await _crepo.ReplaceDynamicFields(request.Replacements, contract);

        return Ok("Dynamic fields updated successfully.");
    }

    [HttpPost("contracts/{id}/generate-pdf")]
    public async Task<IActionResult> GeneratePdf([FromRoute] uint id, [FromBody] UpdateFileRequest request)
    {
        if (request.Replacements is null || request.Replacements.Count == 0)
        {
            return BadRequest("No replacement fields provided.");
        }
        var contract = await _crepo.GetById(id);
        if (contract is null)
        {
            return NotFound("Contract not found.");
        }
        await _crepo.ReplaceDynamicFields(request.Replacements, contract);

        var populatedContract = await _crepo.GetById(id);

        string baseDirectory = AppContext.BaseDirectory;

        string temporaryDocxFilesFolder = Path.Combine(baseDirectory, "TemporaryDocxFiles");
        string pdfFilesFolder = Path.Combine(baseDirectory, "PdfFiles");

        string temporaryDocxFilePath = Path.Combine(temporaryDocxFilesFolder, populatedContract!.Name + ".docx");

        await System.IO.File.WriteAllBytesAsync(temporaryDocxFilePath, populatedContract.FileData);

        FileConverter.ConvertDocxToPdf(temporaryDocxFilePath, Path.Combine(pdfFilesFolder, populatedContract.Name + ".pdf"));

        return Ok("Dynamic fields updated successfully.");
    }

    [HttpGet("contracts/{id}/pdf")]
    public async Task<IActionResult> GetPopulatedContractPdf(uint id)
    {
        var contract = await _crepo.GetById(id);
        if (contract is null)
        {
            return NotFound("Contract not found.");
        }
        var fileName = contract.Name;

        string filePath = Path.Combine(AppContext.BaseDirectory, "PdfFiles", fileName + ".pdf");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

        return File(fileBytes, "application/pdf", $"{contract.Name}.pdf");
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

        string formattedValidFrom = _hmacService.FormatDate(request.ValidFrom);
        string formattedValidUntil = _hmacService.FormatDate(request.ValidUntil);

        return Ok(new { url = $"contracts/{result.Id}?signature={Uri.EscapeDataString(_hmacService.GenerateSignature(request.ValidFrom, request.ValidUntil, id.ToString()))}&validFrom={Uri.EscapeDataString(formattedValidFrom)}&validUntil={Uri.EscapeDataString(formattedValidUntil)}" });
    }
}




