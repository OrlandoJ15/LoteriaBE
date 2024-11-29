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



            // CONFIGURACI�N DE CORS 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MultiplicaDOS", policy =>
                {

                    policy.WithOrigins("http://localhost:5173", "https://multiplicados.net", "https://keyvaultloteria.vault.azure.net/")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            // CONFIGURACI�N DE AZURE KEY VAULT
            var keyVaultUrl = builder.Configuration["AzureKeyVault:VaultUrl"];  //Url del key Vault



            var secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
            
            var jwtSecretKey = secretClient.GetSecret("JwtKey").Value.Value; // Obtiene el secreto desde Key Vault

            var issuer = builder.Configuration["Jwt:Issuer"];
            var audience = builder.Configuration["Jwt:Audience"];


            // CONFIGURACI�N DEL JWT 

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://loteriawebapimvp.azurewebsites.net"; // Aqu� va la URL de tu servidor de autorizaci�n (por ejemplo, Auth0 o Azure AD)
                    options.Audience = audience; // Este es el p�blico esperado por tu API (en Auth0 o AAD, es el identificador de tu API)
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
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
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("Token no v�lido: " + context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("Token validado correctamente.");
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                // Puedes agregar pol�ticas personalizadas aqu� si es necesario
                options.AddPolicy("MultiplicaDOS", policy => policy.RequireAuthenticatedUser());
            });


            // Configuraci�n de controladores y servicios
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configuraci�n del entorno de desarrollo
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("MultiplicaDOS");

            app.UseHttpsRedirection();
            app.UseHsts(); //esto es para ue solo permita coneccion por https - activan el htttp transport security 

            // Autenticaci�n y autorizaci�n
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
