using API.Data;
using API.Data.Repos;
using API.FileConversion;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static API.Mappers.Mappers;
using static API.FileManipulation.FileManipulator;
using OpenXmlPowerTools;

namespace API.Controllers;

[ApiController]
public class ContractController(ContractRepo crepo, TemplateRepo trepo, IHMACService hmacService, UserManager<User> userManager) : ControllerBase
{
    private readonly ContractRepo _crepo = crepo;
    private readonly TemplateRepo _trepo = trepo;
    private readonly IHMACService _hmacService = hmacService;
    private readonly UserManager<User> _userManager = userManager;

    [Authorize]
    [HttpGet("contracts")]
    public async Task<IActionResult> GetContracts()
    {
        var userId = _userManager.GetUserId(User);
        var contracts = await _crepo.GetAll(userId);
        var result = contracts.Select(ToContractDto).ToList();
        return Ok(result);
    }

    [HttpGet("contracts/{id}")]
    public async Task<IActionResult> GetContract(uint id, [FromQuery] string signature, [FromQuery] DateTime validFrom, [FromQuery] DateTime validUntil)
    {
        DateTime utcTodayEnd = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);

        if (validFrom > utcTodayEnd || validUntil < utcTodayEnd) return BadRequest("Invalid valid from or valid until date.");

        if (!_hmacService.IsValidSignature(validFrom, validUntil, id.ToString(), signature)) return BadRequest("Invalid signature");

        var contract = await _crepo.GetById(id);

        if (contract is null) return NotFound("Contract not found.");

        if (contract.SigningStatus != SigningStatus.SignedByNone) return BadRequest("This contract has already been signed.");

        return Ok(ToContractDto(contract));
    }

    [Authorize]
    [HttpGet("contracts/{id}/file")]
    public async Task<IActionResult> GetContractFile(uint id)
    {
        var userId = _userManager.GetUserId(User);
        var contract = await _crepo.GetById(id, userId);
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
        if (contract.SigningStatus != SigningStatus.SignedByNone)
        {
            return BadRequest("Contract has already been signed by atleast one of the parties.");
        }
        if (request.Replacements.Count != contract.Fields.Count)
        {
            return BadRequest("Invalid number of replacement fields provided.");
        }
        if (contract.Fields.Any(f => request.Replacements.All(r => r.Name != f.Name)))
        {
            return BadRequest("Invalid replacement fields provided.");
        }
        var template = await _trepo.GetById(contract.TemplateId);
        if (template is null)
        {
            return NotFound("Template not found.");
        }
        await _crepo.ReplaceDynamicFields(request.Replacements, contract, template);

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
        if (contract.SigningStatus != SigningStatus.SignedByNone)
        {
            return BadRequest("Contract has already been signed by atleast one of the parties.");
        }
        if (request.Replacements.Count != contract.Fields.Count)
        {
            return BadRequest("Invalid number of replacement fields provided.");
        }
        if (contract.Fields.Any(f => request.Replacements.All(r => r.Name != f.Name)))
        {
            return BadRequest("Invalid replacement fields provided.");
        }
        var template = await _trepo.GetById(contract.TemplateId);
        if (template is null)
        {
            return NotFound("Template not found.");
        }
        await _crepo.ReplaceDynamicFields(request.Replacements, contract, template);

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

    [Authorize]
    [HttpDelete("contracts/{id}")]
    public async Task<IActionResult> DeleteContract(uint id)
    {
        var userId = _userManager.GetUserId(User);
        var contract = await _crepo.GetById(id, userId);
        if (contract is null)
        {
            return NotFound("Contract not found.");
        }
        await _crepo.Delete(contract);
        return Ok("Contract deleted successfully.");
    }

    [Authorize]
    [HttpPost("contracts/{id}/url")]
    public async Task<IActionResult> GetContractLink(uint id, [FromBody] GenerateContractLinkRequest request)
    {
        var userId = _userManager.GetUserId(User);
        if (userId is null)
        {
            return BadRequest("User not found.");
        }
        var template = await _trepo.GetById(id, userId);
        if (template is null)
        {
            return NotFound("Template not found.");
        }
        var contract = new Contract
        {
            Name = request.Name,
            FileData = template.FileData,
            Fields = template.Fields.Select(ToContractDynamicField).ToList(),
            SigningStatus = SigningStatus.SignedByNone,
            LinkValidFrom = request.ValidFrom,
            LinkValidUntil = request.ValidUntil,
            TemplateId = template.Id,
        };

        var result = await _crepo.Save(contract, userId);

        string formattedValidFrom = _hmacService.FormatDate(request.ValidFrom);

        DateTime validUntil = request.ValidUntil == DateTime.MinValue ? DateTime.MaxValue : request.ValidUntil;
        string formattedValidUntil = _hmacService.FormatDate(validUntil);

        var url = $"contracts/{result.Id}?signature={Uri.EscapeDataString(_hmacService.GenerateSignature(request.ValidFrom, validUntil, result.Id.ToString()))}&validFrom={Uri.EscapeDataString(formattedValidFrom)}&validUntil={Uri.EscapeDataString(formattedValidUntil)}";

        await _crepo.Update(result.Id, new UpdateContract
        {
            Url = url,
        });

        return Ok(new { url });
    }

    [HttpPost("contracts/{id}/sign")]
    public async Task<IActionResult> SignContract(uint id, [FromForm] IFormFile file)
    {
        var parsedFile = ParseAsiceFile(file, out var exception);

        if (exception != "") return BadRequest(exception);

        var contract = await _crepo.GetById(id);

        if (contract == null) return NotFound($"Contract with id {id} does not exist");

        // siia tulevad type checkid sisseloginud kasutaja rolli põhjal, praegu lihtsalt hard-coded type= candidate.
        //if authenticated user (see kes üles laeb signatuuriga contracti) is admin then type = ContractSignatureType.CompanyRepresentative
        var contractSignature = new ContractSignature
        {
            FileData = parsedFile,
            ContractId = id,
            Type = ContractSignatureType.Candidate
        };

        await _crepo.SaveSignatureAndUpdateContractStatus(contractSignature);

        return Ok("Hey");
    }
}




