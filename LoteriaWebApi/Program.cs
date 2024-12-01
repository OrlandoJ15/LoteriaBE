using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Serilog;


namespace LoteriaWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuración de Kestrel para especificar el puerto
            builder.WebHost.ConfigureKestrel(options =>
            {
                // Configuración para habilitar HTTP en el puerto 80 (si lo deseas)
                //options.ListenAnyIP(80); // HTTP - Puerto 80

                // Configuración para habilitar HTTPS en el puerto 443
                /*options.ListenAnyIP(443, listenOptions =>
                {
                    // Si tienes un certificado .pfx, puedes configurar el certificado SSL aquí
                    // Puedes usar un certificado autofirmado durante el desarrollo
                    listenOptions.UseHttps("path/to/certificate.pfx", "yourCertificatePassword"); // Configura el certificado SSL
                });*/
            });


            builder.Services.AddHealthChecks();


            // CONFIGURACIÓN DE CORS 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("LoteriaBackApi", policy =>
                {

                    policy.WithOrigins("http://localhost:5173", "https://multiplicados.net")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // CONFIGURACIÓN DE AZURE KEY VAULT
            var keyVaultUrl = builder.Configuration["AzureKeyVault:VaultUrl"];  //Url del key Vault
            var secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());         
            var jwtSecretKey = secretClient.GetSecret("JwtKey").Value.Value; // Obtiene el secreto desde Key Vault
            var issuer = builder.Configuration["Jwt:Issuer"];
            var audience = builder.Configuration["Jwt:Audience"];

            // CONFIGURACIÓN DEL JWT 

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://loteriabackapi-djhxctfjhdg5csfm.centralus-01.azurewebsites.net"; // Aquí va la URL de tu servidor de autorización (por ejemplo, Auth0 o Azure AD)
                    options.Audience = audience; // Este es el público esperado por tu API (en Auth0 o AAD, es el identificador de tu API)
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,    
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
                        ValidIssuer = issuer,  // Si usas un issuer válido, añádelo aquí
                        ValidAudience = audience, // Lo mismo para el audience
                        ClockSkew = TimeSpan.Zero,
                    };
                    
                    /*options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // Leer el token desde la cookie
                            var accessToken = context.Request.Cookies["Token"];
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };*/
                    
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            //Log.Error("Autenticación fallida: {Error}", context.Exception.GetType().ToString());
                            Console.WriteLine("Token no válido: " + context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            //Log.Information("Token validado correctamente para el usuario {UserId}", context.Principal.Identity.Name);
                            Console.WriteLine("Token validado correctamente.");
                            return Task.CompletedTask;
                        }   
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                // Puedes agregar políticas personalizadas aquí si es necesario
                options.AddPolicy("LoteriaBackApi", policy =>
                    policy.RequireAuthenticatedUser());
            });


            // Configuración de controladores y servicios
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443; // Redirige el tráfico HTTP al puerto 443
            });

            var app = builder.Build();

            // Configuración del entorno de desarrollo
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHsts();
            }

            app.MapHealthChecks("/api/health");

            app.UseCors();

            app.UseHttpsRedirection();

            // Autenticación y autorización
            app.UseAuthentication();
            app.UseAuthorization();

            // Habilitar el enrutamiento de API
            app.UseRouting();

            app.MapControllers();

            app.Run();
        }
    }
}
