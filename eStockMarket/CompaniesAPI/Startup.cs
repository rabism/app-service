using AutoWrapper;
using CompaniesAPI.DBContexts;
using CompaniesAPI.Repository;
using CompaniesAPI.Services;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prometheus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompaniesAPI
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
            string server = Environment.GetEnvironmentVariable("SQL_HOST");
            string database = Environment.GetEnvironmentVariable("SQL_DB");
            string userId = Environment.GetEnvironmentVariable("SQL_USER");
            string password = Environment.GetEnvironmentVariable("SQL_PASSWORD");
            string winAuth = Environment.GetEnvironmentVariable("SQL_INTEGRATED_SECURITY");

            string connectionString = string.Empty;
            if (winAuth == "Y")
            {
                connectionString = string.Format("server={0};database={1};Integrated security={2}", server, database, true);
            }
            else
            {
                connectionString = string.Format("server={0};database={1};User ID={2};Password={3};", server, database, userId, password);
            }
            services.AddDbContext<CompanyDBContext>(o => o.UseSqlServer(connectionString));
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Company API", Version = "v1" });
            });
            string kafkaHost = Environment.GetEnvironmentVariable("KAFKA_HOST");
            string kafkaPort = Environment.GetEnvironmentVariable("KAFKA_PORT");
            string kafkaGroup = Environment.GetEnvironmentVariable("KAFKA_GROUP_ID");
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = string.Format("{0}:{1}", kafkaHost, kafkaPort),
                GroupId = kafkaGroup,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                //AutoCommitIntervalMs = 8000
            };

            services.AddSingleton<ConsumerConfig>(consumerConfig);
            services.AddHostedService<MessageReceiver>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Custom Metrics to count requests for each endpoint and the method
            var counter = Metrics.CreateCounter("companyapi_path_counter", "Counts requests to the Company API endpoints", new CounterConfiguration
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company API V1");
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
