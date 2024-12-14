using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Data.Repos;
using API.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using API.emails;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services
    .AddDbContext<DataContext>(options => options.UseSqlite("Data Source=localdatabase.db"))
    .AddScoped<ContractRepo>()
    .AddScoped<IHMACService, HMACService>()
    .AddScoped<TemplateRepo>()
    .AddScoped<ISettingsRepo, SettingsRepo>()
    .AddScoped<IEmailsService, EmailsService>();

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
    {
        builder
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader();
    }));

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<DataContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1;
});

var app = builder.Build();

using (var scope = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
using (var context = scope.ServiceProvider.GetService<DataContext>())
{
    context?.Database.EnsureCreated();

    if (app.Environment.EnvironmentName == "Testing")
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var testUser = new User
        {
            UserName = "testUser@gmail.com",
            Email = "testUser@gmail.com"
        };
        await userManager.CreateAsync(testUser, "testPassword");

        var template = DataBaseSeeder.TemplateFromFileAsync("TestTemplate.docx").Result;
        testUser.Templates.Add(template!);
        var template2 = DataBaseSeeder.TemplateFromFileAsync("apitest.docx").Result;
        testUser.Templates.Add(template2!);

        await userManager.UpdateAsync(testUser);
        context?.SaveChanges();
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");

CreateRequiredDirectories();

void CreateRequiredDirectories()
{
    string baseDirectory = AppContext.BaseDirectory;

    string temporaryDocxFilesPath = Path.Combine(baseDirectory, "TemporaryDocxFiles");
    string pdfFilesPath = Path.Combine(baseDirectory, "PdfFiles");
    string uploadedContractsPath = Path.Combine(baseDirectory, "SignedContracts");
    var tempAsicePath = Path.Combine(baseDirectory, "tempAsiceFiles");

    Directory.CreateDirectory(temporaryDocxFilesPath);
    Directory.CreateDirectory(pdfFilesPath);
    Directory.CreateDirectory(uploadedContractsPath);
    Directory.CreateDirectory(tempAsicePath);

    Console.WriteLine($"Created directories: {temporaryDocxFilesPath}, {pdfFilesPath}, {uploadedContractsPath}, {tempAsicePath}");
}
app.MapIdentityApi<User>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
