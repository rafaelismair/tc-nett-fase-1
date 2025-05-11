using Fiap.CloudGames.Fase1.API.Middleware;
using Fiap.CloudGames.Fase1.API.Middleware.Logging;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Fiap.CloudGames.Fase1.Application.Services;
using Fiap.CloudGames.Fase1.Infrastructure.Data;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Services;
using Fiap.CloudGames.Fase1.Infrastructure.Middleware.CustomException;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddCorrelationIdGenerator();
builder.Services.AddTransient(typeof(ILogService<>), typeof(LogService<>));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IGameService, GameService>();


builder.Services.AddAuthorization();

builder.Services.AddControllers(config =>
{
    config.Filters.Add(typeof(CustomExceptionMiddleware));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "FIAP Cloud Games", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#region Middlewares
app.UseCorrelationIdMiddleware();
#endregion

app.Run();
