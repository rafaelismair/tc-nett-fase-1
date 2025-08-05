using Fiap.CloudGames.Fase1.API.Extensions;
using Fiap.CloudGames.Fase1.API.Middleware;
using Fiap.CloudGames.Fase1.API.Middleware.ErrorHandling;
using Fiap.CloudGames.Fase1.API.Middleware.Logging;
using Fiap.CloudGames.Fase1.Application.Interfaces;
using Fiap.CloudGames.Fase1.Application.Services;
using Fiap.CloudGames.Fase1.Infrastructure.Data;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Interfaces;
using Fiap.CloudGames.Fase1.Infrastructure.LogService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Prometheus;
using System.Text;

Log.Logger = SerilogConfiguration.ConfigureSerilog();

var builder = WebApplication.CreateBuilder(args);

// 🔹 Adiciona coleta de métricas e health checks
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Host.UseSerilog(); // Usa Serilog como logger principal

// 🔹 Banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 JWT
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

builder.Services.AddAuthorization();

// 🔹 Injeções personalizadas
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddTransient(typeof(ILogService<>), typeof(LogService<>));
builder.Services.AddCorrelationIdGenerator();

// 🔹 Swagger + JWT
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "FIAP Cloud Games", Version = "v1" });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu token}"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Fiap.CloudGames.Fase1.API.xml"));
});

var app = builder.Build();

// 🔹 Swagger
app.UseSwagger();
app.UseSwaggerUI();

// 🔹 Migrações do banco
app.ApplyMigrations();

// 🔹 Middlewares padrão
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

// 🔹 Prometheus
app.UseHttpMetrics();     // 🔁 ❌ Removida duplicação
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapMetrics(); // <-- expõe /metrics
});

// 🔹 Middlewares personalizados
app.UseCorrelationIdMiddleware();
app.UseErrorHandlingMiddleware();

app.Run();
