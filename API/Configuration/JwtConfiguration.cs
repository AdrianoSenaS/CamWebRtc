using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System.Text;

namespace CamWebRtc.API.Configuration
{
    public static class JwtConfiguration
    {
        public static void AddJwtConfiguration(this IServiceCollection services, WebApplicationBuilder builder)
        {

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                }
                catch (Exception)
                {

                }
            });

        }
    }
}
