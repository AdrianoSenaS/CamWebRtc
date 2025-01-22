using Microsoft.AspNetCore.Cors;
namespace CamWebRtc.API.Configuration
{
    /// <summary>
    /// Configuração personalizada Cors 
    /// Essa configuração usa Origin para todos
    /// OBS: Se for usar em produção recomendo usar Origin para domínios
    /// </summary>
    public static class CorsConfig
    {
        /// <summary>
        /// Função de configuração Cors
        /// </summary>
        public static void AddCrosConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                    policy.AllowAnyOrigin()   // Permite qualquer origem
                    .AllowAnyHeader()   // Permite qualquer cabeçalho
                    .AllowAnyMethod()
                );  // Permite qualquer método
            });
        }
    }
}
