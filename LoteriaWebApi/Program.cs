
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
