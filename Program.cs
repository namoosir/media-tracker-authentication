using MediaTrackerAuthenticationService;
using MediaTrackerAuthenticationService.Data;
using MediaTrackerAuthenticationService.Models;
using MediaTrackerAuthenticationService.Services.PlatformConnectionService;
using MediaTrackerAuthenticationService.Services.AuthService;
using MediaTrackerAuthenticationService.Services.RequestUrlBuilderService;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddCors();

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString"))
);

builder.Services.AddSingleton<IConnectionMultiplexer>(
    opt =>
        ConnectionMultiplexer.Connect(
            builder.Configuration.GetConnectionString("RedisConnectionString")
        )
);

builder.Services.AddScoped<IUserInformationRepository, UserInformationRepository>();

builder.Services.AddSingleton(builder.Configuration.GetSection("GoogleOauth"));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowSpecificOrigins",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpClient();
builder.Services.AddScoped<IPlatformConnectionService, PlatformConnectionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRequestUrlBuilderService, RequestUrlBuilderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseCors(builder => {
//     builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
// });
app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
