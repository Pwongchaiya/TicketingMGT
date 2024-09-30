using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicketMGT.Core.Api.Brokers.DateTimeBrokers;
using TicketMGT.Core.Api.Brokers.DateTimes;
using TicketMGT.Core.Api.Brokers.Loggings;
using TicketMGT.Core.Api.Brokers.Storages;
using TicketMGT.Core.Api.Services.Foundations;

namespace YourNamespace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            builder.Services.AddDbContext<StorageBroker>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddProblemDetails();

            // Configure IConfiguration
            builder.Configuration.AddJsonFile(
                "appsettings.json",
                optional: false,
                reloadOnChange: true);

            // Register the brokers
            builder.Services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();
            builder.Services.AddTransient<IStorageBroker, StorageBroker>();
            builder.Services.AddTransient<ITicketService, TicketService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:3001")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseCors("AllowLocalhost");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}
