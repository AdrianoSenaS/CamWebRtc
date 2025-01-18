using Microsoft.OpenApi.Models;
namespace CamWebRtc.API.Configuration
{
    /// <summary>
    /// Configuração pernosalizado o Swagger
    /// Essas configurações server para informar a versão da api,
    /// titulo e informações da empresa e contatos 
    /// </summary>
    public static class SwaggerConfig
    {
        /// <summary>
        /// Definindo variaveis para usar nas configurações do Swagger
        /// </summary>

        public static string SwaggerPrefix = "swagger";
        public static string SwaggerVersion = "V1";
        public static string SwaggerTitle = "Api Cam WebRtv";
        public static string SwaggerDescription = "Api para gerenciamento de cameras";
        public static string SwaggerName = "Adriano Sena Silva";
        public static string SwaggerEmail = "adryanosenasilva@gmail.com";
        public static string SwaggerEndPoint = $"/swagger/{SwaggerVersion}/swagger.json";

        /// <summary>
        /// Adicionando configuração do SwaggerGen
        /// </summary>
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(SwaggerVersion, new OpenApiInfo 
                {
                    Title = SwaggerTitle,
                    Version = SwaggerVersion,
                    Description = SwaggerDescription,
                    Contact = new OpenApiContact
                    {
                        Email = SwaggerEmail,
                        Name = SwaggerName,
                    }
                });
            });
        }

        /// <summary>
        /// Adicionando Configuração SwaggerUI
        /// </summary>
        public static void UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(SwaggerEndPoint, $"{SwaggerTitle} {SwaggerVersion}");
                options.RoutePrefix = SwaggerPrefix;
            });
        }
    }
}
