
using AccesoDatos.DBContext;
using AccesoDatos.Implementacion;
using AccesoDatos.Interfaz;
using LogicaNegocio.Implementacion;
using LogicaNegocio.Interfaz;
using Microsoft.EntityFrameworkCore;

namespace LoteriaWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //var connectionString = builder.Configuration.GetConnectionString("LoteriaBD");

            //builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            //if (string.IsNullOrEmpty(connectionString))
            //{
            //    throw new InvalidOperationException("La cadena de conexión 'gCnnBD' no se encuentra en appsettings.json.");
            //}


            //builder.Services.AddDbContext<LoteriaContext>(options =>
              //  options.UseSqlServer(connectionString));


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PermitirFrontEnd",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173") // Dirección de tu frontend
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("PermitirFrontEnd");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
            
        }
    }
}
