using AutoWrapper;
using Confluent.Kafka;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prometheus;
using StocksAPI.Models;
using StocksAPI.Repository;
using StocksAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StocksAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<Settings>(Configuration.GetSection("MongoConnection"));
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IMessageProducerService, MessageProducerService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            services.AddScoped<IStockDbContext,StockDbContext>();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtConfig:Secret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Stock API", Version = "v1" });
            });
            //Kafka configuration binding
            string kafkaHost = Environment.GetEnvironmentVariable("KAFKA_HOST");
            string kafkaPort = Environment.GetEnvironmentVariable("KAFKA_PORT");
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = string.Format("{0}:{1}", kafkaHost, kafkaPort)
            };
            services.AddSingleton<ProducerConfig>(producerConfig);
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Custom Metrics to count requests for each endpoint and the method
            var counter = Metrics.CreateCounter("stockapi_path_counter", "Counts requests to the Stock API endpoints", new CounterConfiguration
            {
                LabelNames = new[] { "method", "endpoint" }
            });
            app.Use((context, next) =>
            {
                counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
                return next();
            });
            // Use the Prometheus middleware
            app.UseMetricServer();
            app.UseHttpMetrics();
            app.UseCors("CorsPolicy");
            app.UseApiResponseAndExceptionWrapper();
            app.UseRouting();


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock API V1");
            });

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
