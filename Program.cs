using CamWebRtc.API.Configuration;
using CamWebRtc.Application.Interfaces;
using CamWebRtc.Application.Services;
using CamWebRtc.Infrastructure.Config;
using CamWebRtc.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Adicionando a configuração personalizada do Swagger
builder.Services.AddSwaggerConfiguration();
//Adicionando configuração personalizada do Cors
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
var app = builder.Build();
// Configurar o caminho para arquivos estáticos (Frontend)
var frontendPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //Adicionando configuração personalizado do SwaggerUi
    app.UseSwaggerConfiguration();
}
//Usando cors
app.UseCors("AllowAll");
// Servir arquivos estáticos do diretório Frontend
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(frontendPath),
 
});
//Criando Scope
var scope = app.Services.CreateScope();
//Adicionando Migrations
scope.ServiceProvider.AppMigrations();
//Adicionando Controladores
app.MapControllers();
//Adicionando autorização
app.UseAuthorization();
//Usando rotas
app.UseRouting();
//Adicioando conttrolles
app.MapControllers();
//Iniciando aplicação
app.Run();
