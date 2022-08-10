using IoTGateway.Data;
using IoTGateway.Services.Implementations;
using IoTGateway.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IoTGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseInMemoryDatabase("IoT"));
            builder.Services.AddTransient<IGatewayService, GatewayService>();
            builder.Services.AddTransient<IPeripheralService, PeripheralService>();
            builder.Services.AddControllers().AddJsonOptions(o =>
            {
                //o.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                //o.JsonSerializerOptions.MaxDepth = 4;
            });

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
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}