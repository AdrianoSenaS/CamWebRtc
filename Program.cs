using CamWebRtc.API.Configuration;
using CamWebRtc.Application.Interfaces;
using CamWebRtc.Application.Services;
using CamWebRtc.Infrastructure.Config;
using CamWebRtc.Infrastructure.Data;
using CamWebRtc.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//Adicionando SignalR
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Adicionando a configura��o personalizada do Swagger
builder.Services.AddSwaggerConfiguration();
//Adicionando configura��o personalizada do Cros
builder.Services.AddCrosConfiguration();
//Adicionado Banco de dados 
builder.Services.AppConfigureDb();
//Adicionando Escopos
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ICamRepository, CamRepository>();
builder.Services.AddScoped<CamService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //Adicionando configura��o personalizado do SwaggerUi
    app.UseSwaggerConfiguration();
}
//Adicionando Migrations
var scope = app.Services.CreateScope();
scope.ServiceProvider.AppMigrations();
//Usando redirecionamento Https
//app.UseHttpsRedirection();
//Usando Autoriza��o
app.UseAuthorization();
//Adicionando Controladores
app.MapControllers();
//Usando do Cros personalizado
app.UseCors("AllowAll");
//adiconando webrtc signaling
app.MapHub<WebRTCSignalingHub>("/webrtc-signaling");
//Iniciando aplica��o
app.Run();
