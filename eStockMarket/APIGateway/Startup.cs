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
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway
{
    public class Startup
    {
        readonly string GatewayClient;
        public Startup(IWebHostEnvironment environment)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(environment.ContentRootPath).AddJsonFile("configuration.json", optional: false).AddJsonFile("appsettings.json", optional: false).AddEnvironmentVariables();
            Configuration = builder.Build();
            GatewayClient = Configuration.GetSection("GatewayClient").Value;
        }
        public IConfigurationRoot Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            this.ValidateToken(Configuration, services);
            services.AddOcelot(Configuration);
            services.AddCors(options =>
            {
                options.AddPolicy(name: "RestrictedAccess",
                builder =>
                {
                    builder.WithOrigins(GatewayClient).AllowAnyHeader().AllowAnyMethod();
                });
            });
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gateway", Version = "v1" });
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseAuthentication();
            //app.UseAuthorization();
            //app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway");
            //});
            app.UseCors("RestrictedAccess");
            app.UseOcelot().Wait();
        }

        private void ValidateToken(IConfiguration configuration, IServiceCollection services)
        {
            var audienceConfig = configuration.GetSection("Audience");
            var key = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(key);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                ValidateAudience = false,
                ValidateIssuer = false,

                //ValidateLifetime = true,
                //ClockSkew = TimeSpan.Zero,
                
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("basic", o => {
                o.TokenValidationParameters = tokenValidationParameters;
                o.SaveToken = true;
                o.RequireHttpsMetadata = false;
            });
        }
    }
}
