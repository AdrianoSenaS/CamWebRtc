using System.Text;
using CamWebRtc.API.Configuration;
using CamWebRtc.Application.Interfaces;
using CamWebRtc.Application.Services;
using CamWebRtc.Infrastructure.Config;
using CamWebRtc.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);

// Adicionando serviços ao container
builder.Services.AddControllers();
// Adicione os serviços necessários
builder.Services.AddSignalR();
// Configurar autenticação JWT
builder.Services.AddJwtConfiguration(builder);
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
// Configuração de arquivos estáticos
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(frontendPath),
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<SignalingHub>("/signaling");
});
// Iniciando a aplicação
app.Run();
