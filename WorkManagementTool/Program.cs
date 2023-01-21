using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using WorkManagementTool.Data;
using WorkManagementTool.Models.Configs;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;


builder.Services.AddDbContext<WorkManagementToolContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("WorkManagementToolContext") ?? throw new InvalidOperationException("Connection string 'WorkManagementToolContext' not found.")));
// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(option =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "GIS API",
        Description = "ASP.NET Core Web API"
    });
});

//builder.Services.AddAuthorization();

services.Configure<JournalConfigs>(configuration.GetSection(nameof(JournalConfigs)));
services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
});




var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();