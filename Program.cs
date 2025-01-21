using CamWebRtc;
using CamWebRtc.API.Configuration;
using CamWebRtc.Application;
using CamWebRtc.Application.Interfaces;
using CamWebRtc.Application.Services;
using CamWebRtc.Infrastructure.Config;
using CamWebRtc.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Adicionando a configuração personalizada do Swagger
builder.Services.AddSwaggerConfiguration();
//Adicionando configuração personalizada do Cros
builder.Services.AddCrosConfiguration();
//Adicionado Banco de dados 
builder.Services.AppConfigureDb();
//Adicionando Escopos
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ICamRepository, CamRepository>();
builder.Services.AddScoped<CamService>();
builder.Services.AddScoped<IStreamRepository, StreamRepository>();
builder.Services.AddScoped<StreamServices>();
var nodeService = new NodeJsService();
var nodeScriptPath = "wwwroot/Server.js"; // Caminho do arquivo server.js
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //Adicionando configuração personalizado do SwaggerUi
    app.UseSwaggerConfiguration();
}
app.UseCors("AllowAll");
//Adicionando Migrations
var scope = app.Services.CreateScope();
scope.ServiceProvider.AppMigrations();
// Servir arquivos estáticos do diretório Frontend
//Adicionando Controladores
app.MapControllers();
app.UseAuthorization();
app.UseStaticFiles();
app.Lifetime.ApplicationStarted.Register(() =>
{
    nodeService.StartNodeServer(nodeScriptPath);
});

app.Lifetime.ApplicationStopping.Register(() =>
{
    nodeService.StopNodeServer();
});
app.MapGet("/", () => "ASP.NET + Node.js Integration!");
app.UseRouting();
app.MapControllers();
//Iniciando aplicação
app.Run();
