using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Data.Repos;
using API.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services
    .AddDbContext<DataContext>(options => options.UseSqlite("Data Source=localdatabase.db"))
    .AddScoped<ContractRepo>()
    .AddScoped<IHMACService, HMACService>()
    .AddScoped<TemplateRepo>();

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
    {
        builder
        .SetIsOriginAllowed(_ => true)
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader();
    }));

var app = builder.Build();

using (var scope = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
using (var context = scope.ServiceProvider.GetService<DataContext>())
{
    context?.Database.EnsureCreated();
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

    Directory.CreateDirectory(temporaryDocxFilesPath);
    Directory.CreateDirectory(pdfFilesPath);

    Console.WriteLine($"Created directories: {temporaryDocxFilesPath}, {pdfFilesPath}");
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();
