using Microsoft.AspNetCore.Cors;
namespace CamWebRtc.API.Configuration
{
    /// <summary>
    /// Configuração personalizada Cros 
    /// Essa configuração usa Origin para todos
    /// OBS: Se for usar em produção recomendo usar Origin para domínios
    /// </summary>
    public static class CrosConfig
    {
        /// <summary>
        /// Função de configuração Cros
        /// </summary>
        public static void AddCrosConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins("http://127.0.0.1:5500") // O domínio do seu cliente
                    .AllowCredentials() // Permitir o envio de credenciais (cookies, autenticação)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }
    }
}
