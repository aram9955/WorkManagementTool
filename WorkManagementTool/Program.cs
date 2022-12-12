using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using WorkManagementTool.Data;
using WorkManagementTool.Models.Configs;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddDbContext<WorkManagementToolContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("WorkManagementToolContext") ?? throw new InvalidOperationException("Connection string 'WorkManagementToolContext' not found.")));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
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

builder.Services.Configure<JournalConfigs>(configuration.GetSection(nameof(JournalConfigs)));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();