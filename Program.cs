using System.Text;
using CamWebRtc.API.Configuration;
using CamWebRtc.Application.Interfaces;
using CamWebRtc.Application.Services;
using CamWebRtc.Infrastructure.Config;
using CamWebRtc.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;

var builder = WebApplication.CreateBuilder(args);

// Adicionando servi�os ao container
builder.Services.AddControllers();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");

// Configurar autentica��o JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        try
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!))
            };
            // Eventos para lidar com falhas de autentica��o
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(new
                    {
                        error = "Token inv�lido ou expirado",
                        details = context.Exception.Message
                    }.ToJson());
                },
                OnChallenge = context =>
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(new
                    {
                        error = "Token ausente ou inv�lido",
                        details = "Por favor, forne�a um token JWT v�lido no cabe�alho Authorization"
                    }.ToJson());
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = 403; // Forbidden
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(new
                    {
                        error = "Acesso negado",
                        details = "Voc� n�o tem permiss�o para acessar este recurso"
                    }.ToJson());
                }
            };
        }catch(Exception ex)
        {
           
        }
    });

// Configura��o personalizada do Swagger
builder.Services.AddSwaggerConfiguration();

// Configura��o personalizada do Cors
builder.Services.AddCrosConfiguration();

// Adicionado Banco de dados
builder.Services.AppConfigureDb();

// Adicionando escopos
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ICamRepository, CamRepository>();
builder.Services.AddScoped<CamService>();
builder.Services.AddScoped<IStreamRepository, StreamRepository>();
builder.Services.AddScoped<StreamServices>();
builder.Services.AddScoped<IiceServersRepository, IceServersRepository>();
builder.Services.AddScoped<IceServersService>();

// Adicionando autoriza��o
builder.Services.AddAuthorization();

// Adicionando JWT Service
builder.Services.AddScoped<JwtService>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configurar o caminho para arquivos est�ticos (Frontend)
var frontendPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

// Configura��o personalizada do SwaggerUi
app.UseSwaggerConfiguration();

// Configura��o de CORS
app.UseCors("AllowAll");

// Configura��o de arquivos est�ticos
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(frontendPath),
});

// Middleware de roteamento
app.UseRouting();

// Middleware de autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

// Criando scope e aplicando migrations
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.AppMigrations();
}
// Adicionando controladores
app.MapControllers();

// Iniciando a aplica��o
app.Run();
