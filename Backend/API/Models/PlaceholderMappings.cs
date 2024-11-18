namespace API.Models;

public static class PlaceHolderMappings
{
    const string FirstNamePlaceholder = "_firstName_";
    const string LastNamePlaceholder = "_lastName_";
    const string PhoneNumberPlaceholder = "_phoneNumber_";
    const string EmailPlaceholder = "_email_";
    const string SocialSecurityNumberPlaceholder = "_socialSecurityNumber_";
    const string DateOfBirthPlaceholder = "_dateOfBirth_";
    const string AddressPlaceholder = "_address_";
    const string BankAccountNumberPlaceholder = "_bankAccountNumber_";

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
