using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;



namespace LoteriaWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // CONFIGURACIÓN DE CORS 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirFrontEnd", policy =>
                {
<<<<<<< HEAD
                    policy.WithOrigins("http://localhost:5173", "https://white-grass-096de5c10.5.azurestaticapps.net","https://multiplicados.net")
=======
                    policy.WithOrigins("http://localhost:5173", "https://multiplicados.net")
>>>>>>> 015c4ffb48d3b87fc7a839c769bcb85ad1fe8435
                          .AllowCredentials()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            // CONFIGURACIÓN DE AZURE KEY VAULT
            var keyVaultUrl = builder.Configuration["AzureKeyVault:VaultUrl"];
            var secretClient = new SecretClient(new Uri("https://keyvaultloteria.vault.azure.net/"), new DefaultAzureCredential());
            var jwtSecretKey = secretClient.GetSecret("Jwtkey").Value.Value; // Obtiene el secreto desde Key Vault
            var issuer = builder.Configuration["Jwt:Issuer"];

            // CONFIGURACIÓN DEL JWT 
            
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
                    };
                    /*
                    options.Events = new JwtBearerEvents
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
                    };
                    */
                });

            // Configuración de controladores y servicios
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configuración del entorno de desarrollo
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("PermitirFrontEnd");

            app.UseHttpsRedirection();
            app.UseHsts(); //esto es para ue solo permita coneccion por https - activan el htttp transport security 

            // Autenticación y autorización
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
