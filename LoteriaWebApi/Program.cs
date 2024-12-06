using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.DataProtection;

namespace LoteriaWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configuraci�n de logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console() // Registrar en consola
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) // Registrar en archivo
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            // Configuraci�n de Kestrel para manejar HTTPS y puertos
            ConfigureKestrel(builder);

            // Configuraci�n de servicios
            builder.Services.AddHealthChecks();
            ConfigureHttpClient(builder);
            ConfigureCors(builder);
            ConfigureAzureKeyVault(builder);
            ConfigureJwtAuthentication(builder);
            ConfigureAuthorization(builder);
            ConfigureSwagger(builder);

            // Configuraci�n de controladores y servicios
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpsRedirection(options => options.HttpsPort = 443); // Redirige tr�fico HTTP a HTTPS

            var app = builder.Build();

            // Configuraci�n de Swagger y enrutamiento
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Loter�a API V1");
                    c.RoutePrefix = string.Empty; // Esto har� que Swagger est� accesible en la ra�z de la aplicaci�n
                });
            }

            // Mapeo de rutas y middleware
            app.MapHealthChecks("/api/health");
            app.UseCors("AllowSpecificOrigins");
            app.UseHttpsRedirection();

            app.UseRouting(); // Necesario para que los controladores usen las rutas
            app.UseAuthentication(); // Habilita la autenticaci�n
            app.UseAuthorization(); // Habilita la autorizaci�n

            app.MapControllers(); // Mapea los controladores

            app.Run();
        }

        // Configuraci�n de Kestrel
        // Configuraci�n de Kestrel
        private static void ConfigureKestrel(WebApplicationBuilder builder)
        {
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenLocalhost(443, listenOptions =>
                {
                    listenOptions.UseHttps(httpsOptions =>
                    {
                        httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;
                    });
                });

                options.ListenLocalhost(8080);
            });
        }

        // Configuraci�n del HttpClient
        private static void ConfigureHttpClient(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient("HttpClientWithCertValidation")
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                        builder.Environment.IsDevelopment() ? true : sslPolicyErrors == System.Net.Security.SslPolicyErrors.None
                });
        }

        // Configuraci�n de CORS
        private static void ConfigureCors(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                
                options.AddPolicy("AllowSpecificOrigins ", policy =>
                {
                      policy.WithOrigins("https://localhost:8080", "https://multiplicados.net", "https://portal.azure.com")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
        }

        // Configuraci�n de Azure Key Vault
        private static void ConfigureAzureKeyVault(WebApplicationBuilder builder)
        {
            var keyVaultUrl = builder.Configuration["AzureKeyVault:VaultUrl"];
            var secretClient = new SecretClient(new Uri(keyVaultUrl ?? ""), new DefaultAzureCredential());
            var jwtSecretKey = secretClient.GetSecret("KeyStagingLoteria").Value.Value;
            builder.Services.AddSingleton(jwtSecretKey);
        }

        // Configuraci�n de JWT Authentication
        private static void ConfigureJwtAuthentication(WebApplicationBuilder builder)
        {
            var jwtSecretKey = builder.Services.BuildServiceProvider().GetService<string>();
            var issuer = builder.Configuration["JwtI:Issuer"];
            var audience = builder.Configuration["JwtA:Audience"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = null;
                    options.Audience = audience;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = credentials.Key,
                        ValidIssuer = issuer,
                        ValidAudience = audience
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Log.Error($"Error de autenticaci�n: {context.Exception.GetType()} - {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var user = context.Principal?.Identity?.Name ?? "Unknown User";
                            Log.Information($"Token validado correctamente para el usuario: {user}");
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            Log.Warning($"Desaf�o de autenticaci�n: {context.ErrorDescription} - {context.ErrorUri}");
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                            try
                            {
                                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                                var tokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuer = true,
                                    ValidateAudience = true,
                                    ValidateLifetime = false,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey ?? "")),
                                    ValidIssuer = builder.Configuration["JwtI:Issuer"],
                                    ValidAudience = builder.Configuration["JwtA:Audience"]
                                };
                                context.Principal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                            }
                            catch (Exception ex)
                            {
                                Log.Error($"Error al validar el token: {ex.Message}");
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        // Configuraci�n de Autorizaci�n
        private static void ConfigureAuthorization(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AllowAnyOrigin", policy =>
                {
                    policy.RequireAuthenticatedUser(); // Solo permite acceso a usuarios autenticados
                });
            });
        }

        // Configuraci�n de Swagger
        private static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Loteria API",
                    Version = "v1"
                });
            });
        }
    }
}
