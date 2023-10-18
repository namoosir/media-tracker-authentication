using MediaTrackerAuthenticationService.Data;
using MediaTrackerAuthenticationService.Services.PlatformConnectionService;
using MediaTrackerAuthenticationService.Services.AuthService;
using MediaTrackerAuthenticationService.Services.RequestUrlBuilderService;
using MediaTrackerAuthenticationService.Services.HttpRequestService;
using MediaTrackerAuthenticationService.Services.UserService;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using MediaTrackerAuthenticationService.Services.SessionTokenService;
using MediaTrackerAuthenticationService.Controllers;


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

builder.Services.AddScoped<IHttpRequestService, HttpRequestService>();
builder.Services.AddScoped<ISessionTokenService, SessionTokenService>();
builder.Services.AddScoped<IRequestUrlBuilderService, RequestUrlBuilderService>();
builder.Services.AddScoped<IPlatformConnectionService, PlatformConnectionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserInformationRepository, UserInformationRepository>();
builder.Services.AddScoped<UserInformationController>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddSingleton(builder.Configuration.GetSection("GoogleOauth"));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(
//         "AllowSpecificOrigins",
//         builder =>
//         {
//             builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials();
//         }
//     );


// });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

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
// app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
