using MediaTrackerAuthenticationService;
using MediaTrackerAuthenticationService.Models;
using MediaTrackerAuthenticationService.Services.PlatformConnectionService;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);


// var configurationBuilder = new ConfigurationBuilder()
//     .SetBasePath(builder.Environment.ContentRootPath)
//     .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
//     .AddEnvironmentVariables(); // This line adds support for reading environment variables

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString"))
);

builder.Services.AddSingleton(builder.Configuration.GetSection("GoogleOauth"));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpClient();
builder.Services.AddScoped<IPlatformConnectionService, PlatformConnectionService>();

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
