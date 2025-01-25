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

// Adicionando serviços ao container
builder.Services.AddControllers();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");

// Configurar autenticação JWT
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
            // Eventos para lidar com falhas de autenticação
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(new
                    {
                        error = "Token inválido ou expirado",
                        details = context.Exception.Message
                    }.ToJson());
                },
                OnChallenge = context =>
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(new
                    {
                        error = "Token ausente ou inválido",
                        details = "Por favor, forneça um token JWT válido no cabeçalho Authorization"
                    }.ToJson());
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = 403; // Forbidden
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(new
                    {
                        error = "Acesso negado",
                        details = "Você não tem permissão para acessar este recurso"
                    }.ToJson());
                }
            };
        }catch(Exception ex)
        {
           
        }
    });

// Configuração personalizada do Swagger
builder.Services.AddSwaggerConfiguration();

// Configuração personalizada do Cors
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

// Adicionando autorização
builder.Services.AddAuthorization();

// Adicionando JWT Service
builder.Services.AddScoped<JwtService>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configurar o caminho para arquivos estáticos (Frontend)
var frontendPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

// Configuração personalizada do SwaggerUi
app.UseSwaggerConfiguration();

// Configuração de CORS
app.UseCors("AllowAll");

// Configuração de arquivos estáticos
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(frontendPath),
});

// Middleware de roteamento
app.UseRouting();

// Middleware de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Criando scope e aplicando migrations
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.AppMigrations();
}
// Adicionando controladores
app.MapControllers();

// Iniciando a aplicação
app.Run();
