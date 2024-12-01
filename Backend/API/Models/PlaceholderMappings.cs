namespace API.Models;

public static class PlaceHolderMappings
{
    public const string FirstNamePlaceholder = "_firstName_";
    public const string LastNamePlaceholder = "_lastName_";
    public const string PhoneNumberPlaceholder = "_phoneNumber_";
    public const string EmailPlaceholder = "_email_";
    public const string SocialSecurityNumberPlaceholder = "_socialSecurityNumber_";
    public const string DateOfBirthPlaceholder = "_dateOfBirth_";
    public const string AddressPlaceholder = "_address_";
    public const string BankAccountNumberPlaceholder = "_bankAccountNumber_";

    public static readonly Dictionary<PlaceHolderData, string> Fields = new()
    {
        { new() {  Name = "First name", Type = "string" }, FirstNamePlaceholder },
        { new() {  Name = "Last name", Type = "string" }, LastNamePlaceholder },
        { new() {  Name = "Phone number", Type = "number" }, PhoneNumberPlaceholder },
        { new() {  Name = "Email", Type = "email" }, EmailPlaceholder },
        { new() {  Name = "Social security number", Type = "ssn" }, SocialSecurityNumberPlaceholder },
        { new() {  Name = "Date of birth", Type = "date" }, DateOfBirthPlaceholder },
        { new() {  Name = "Address", Type = "string" }, AddressPlaceholder },
        { new() {  Name = "Bank account number", Type = "iban" }, BankAccountNumberPlaceholder }

    };
}
public readonly struct PlaceHolderData
{
    public readonly string Name { get; init; }
    public readonly string Type { get; init; }
}
