using Microsoft.AspNetCore.DataProtection;
using LacunaSpace.API.Config;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDataProtection();

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();


builder.Services.ResolveDependencies();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddApiConfig();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseApiConfig(app.Environment);
app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();
