
using AuthAppDemoService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthAppDemoLog;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using AuthAppDemoDB.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AuthAppDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var jwtInfo = builder.Configuration.GetSection("jwt").Get<JwtInfo>();
            var dbLoggerOptions = builder.Configuration.GetSection("Logging:Database:Options").Get<DbLoggerOptions>();
            IServiceCollection DBServices = new ServiceCollection();

            DBServices.AddDbContext<AuthDemo01Context>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("main")),
                    ServiceLifetime.Scoped);

            //builder.Services.AddSqlServer<AuthDemo01Context>(builder.Configuration.GetConnectionString("main"));

            builder.Services.AddSingleton(jwtInfo);

            // add AuthAppDemoServices Injection by call extension method from AuthAppDemoServices Project
            // I do that to hide Service Impelmention calling form controllers
            builder.Services.AddAuthSerices(jwtInfo, DBServices);

            // Register all application services automatically
            builder.Services.AddApplicationServices();



            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            //var connectionString = builder.Configuration.GetSection("Logging:Database:Options:ConnectionString").Get<string>();
            builder.Logging.AddProvider(new DbLoggerProvider(dbLoggerOptions?.ConnectionString, dbLoggerOptions?.LogTable));


            //builder.Logging.AddDbLogger(options =>
            //{
            //    builder.Configuration.GetSection("Logging")
            //    .GetSection("Database").GetSection("Options").Bind(options);
            //});


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }            
            

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
