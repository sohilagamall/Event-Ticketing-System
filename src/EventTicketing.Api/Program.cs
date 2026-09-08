using EventTicketing.Api.ExceptionHandling;
using EventTicketing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EventTicketing.Infrastructure.Identity;
using EventTicketing.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//configuration -> cnnect to my appsettings.json file and get the connection string for the database
var connectionString = builder.Configuration.
    GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//services -> add the database context to the dependency injection container so that it can be used throughout the application and configure it to use SQL Server with the connection string from the configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAuthenticationInfrastructure(builder.Configuration); // Add authentication infrastructure

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

await app.Services.SeedRolesAsync(); // Seed roles at startup

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
