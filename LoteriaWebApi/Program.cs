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
                options.AddPolicy("MultiplicaDOS", policy =>
                {

                    policy.WithOrigins("http://localhost:5173", "https://multiplicados.net", "https://keyvaultloteria.vault.azure.net/")
                          .AllowCredentials()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            // CONFIGURACIÓN DE AZURE KEY VAULT
            var keyVaultUrl = builder.Configuration["AzureKeyVault:VaultUrl"];
            var secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
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
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],  // Emisor del JWT
                        ValidAudience = builder.Configuration["Jwt:Issuer"], // Audiencia del JWT
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AzureKeyVault:SecretKey"])) // La clave secreta del JWT
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

            app.UseCors("MultiplicaDOS");

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
