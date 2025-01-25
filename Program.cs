using System.Text;
using CamWebRtc.API.Configuration;
using CamWebRtc.Application.Interfaces;
using CamWebRtc.Application.Services;
using CamWebRtc.Infrastructure.Config;
using CamWebRtc.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);

// Adicionando servi�os ao container
builder.Services.AddControllers();
// Adicione os servi�os necess�rios
builder.Services.AddSignalR();
// Configurar autentica��o JWT
builder.Services.AddJwtConfiguration(builder);
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
// Configura��o de arquivos est�ticos
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(frontendPath),
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<SignalingHub>("/signaling");
});
// Iniciando a aplica��o
app.Run();
