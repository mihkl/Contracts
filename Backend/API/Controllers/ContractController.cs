using API.Data;
using API.Data.Repos;
using API.FileConversion;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static API.Mappers.Mappers;
using static API.FileManipulation.FileManipulator;
using API.Validation;
using API.emails;

namespace API.Controllers;

[ApiController]
public class ContractController(ContractRepo crepo, TemplateRepo trepo, IHMACService hmacService, UserManager<User> userManager, ISettingsRepo settingsRepo, IEmailsService emailsService) : ControllerBase
{
    private readonly ContractRepo _crepo = crepo;
    private readonly TemplateRepo _trepo = trepo;
    private readonly IHMACService _hmacService = hmacService;
    private readonly UserManager<User> _userManager = userManager;
    private readonly ISettingsRepo _settingsRepo = settingsRepo;
    private readonly IEmailsService _emailsService = emailsService;

    [Authorize]
    [HttpGet("contracts")]
    public async Task<IActionResult> GetContracts([FromQuery] SigningStatus? status)
    {
        var userId = _userManager.GetUserId(User);
        var contracts = await _crepo.GetAll(userId, status);
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

        var validationResult = await _crepo.ReplaceDynamicFields(request.Replacements, contract, template);
        if (validationResult != string.Empty)
        {
            return BadRequest(validationResult);
        }

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

    [HttpGet("contracts/{id}/signed-contract")]
    [Authorize]
    public async Task<IActionResult> GetSignedContract(uint id, [FromQuery] ContractSignatureType signatureType)
    {
        var contract = await _crepo.GetById(id);

        if (contract is null)
        {
            return NotFound("Contract not found.");
        }

        if (contract.SigningStatus == SigningStatus.SignedByNone)
        {
            return BadRequest("This contract has not been signed");
        }

        var signature = contract.Signatures.FirstOrDefault(s => s.Type == signatureType);

        if (signature == null) return BadRequest("This contract does not have any matching signatures.");

        var bytes = await System.IO.File.ReadAllBytesAsync(signature.FilePath);

        var contentType = "application/vnd.etsi.asic-e+zip";
        return new FileContentResult(bytes, contentType)
        {
            FileDownloadName = "contract.asice"
        };
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

    [Authorize]
    [AllowAnonymous]
    [HttpPost("contracts/{id}/sign")]
    public async Task<IActionResult> SignContract(uint id, [FromForm] IFormFile file)
    {
        var parsedFile = ParseAsiceFile(file, out var exception);

        if (exception != "") return BadRequest(exception);

        var contract = await _crepo.GetById(id);

        if (contract == null) return NotFound($"Contract with id {id} does not exist");

        string? userId = _userManager.GetUserId(User);

        var asiceValidator = new AsiceValidator();
        var signatureValidationResult = await asiceValidator.ValidateSignatures(file, userId);
        var contentValidationResult = asiceValidator.ValidateContent(contract);

        if (!signatureValidationResult) return BadRequest("Missing or invalid signature(s)");

        if (!contentValidationResult) return BadRequest("File content does not match contract");

        var contractSignatureType = userId != null ?
            ContractSignatureType.CompanyRepresentative
            : ContractSignatureType.Candidate;

        await _crepo.SaveSignature(contract, parsedFile, contractSignatureType);

        var companyUserId = userId ?? await _crepo.GetUserByContractId(id);

        if (contractSignatureType == ContractSignatureType.Candidate)
        {
            var companyRepresentativeEmail = await _settingsRepo.GetSignedContractUploadNotificationEmailAddress(companyUserId!);

            if (!string.IsNullOrWhiteSpace(companyRepresentativeEmail))
            {
                var emailContent = await _settingsRepo.GetSendSignedContractUploadEmailContentAndSubject(companyUserId!);

                if (!string.IsNullOrWhiteSpace(companyRepresentativeEmail))
                {
                    EmailMessage message = new EmailMessage(companyRepresentativeEmail, DateTime.UtcNow, EmailMessage.EmailMessageType.SignedContractReceivedNotification, emailContent.Value.content, emailContent.Value.subject);
                    await _emailsService.SendEmailsAsync(new List<EmailMessage> { message }, companyUserId!);
                }
            }
        }

        else if (contractSignatureType == ContractSignatureType.CompanyRepresentative)
        {
            var sendFinalContractEmail = await _settingsRepo.GetSendFinalContractEmailContentAndSubject(companyUserId!);

            if (!string.IsNullOrEmpty(sendFinalContractEmail.Value.content) && !string.IsNullOrEmpty(sendFinalContractEmail.Value.subject))
            {
                var applicantEmail = contract.SubmittedFields.FirstOrDefault(x => x.Name == "Email")?.Value;

                if (!string.IsNullOrEmpty(applicantEmail))
                {
                    EmailMessage message = new EmailMessage(applicantEmail, DateTime.UtcNow, EmailMessage.EmailMessageType.FinalContractNotification, sendFinalContractEmail.Value.content, sendFinalContractEmail.Value.subject);
                    await _emailsService.SendEmailsAsync(new List<EmailMessage> { message }, companyUserId!);
                }
            }
        }

        return Ok();
    }
}




