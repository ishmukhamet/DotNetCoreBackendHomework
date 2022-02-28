using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using WebApi.BusinessLogic.RequestHandlers;
using WebApi.Engine;
using WebApi.Queue;
using WebApi.Storage;
using WebApi.Storage.Contracts.Repositories;
using WebApi.Storage.Repositories;

namespace WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Обработчики запросов
            services.AddScoped<AddTodoItemRequestHandler>();
            services.AddScoped<UpdateTodoItemRequestHandler>();
            services.AddScoped<GetTodoItemRequestHandler>();
            services.AddScoped<GetTodoItemListRequestHandler>();

            // Хранилища
            services.AddDbContext<AppDbContext>(opts =>
            {
                opts.UseNpgsql(_configuration["ConnectionStrings:postgres"]);
            });
            services.AddScoped<ITodoItemRepository, TodoItemRepository>();

            var rabbitMqSection = _configuration.GetSection("RabbitMq");
            var url = rabbitMqSection.GetValue<string>("Url");

            services.AddMassTransit(x =>
            {
                x.AddConsumer<UpdateTodoItemMessageConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(url);
                    cfg.ConfigureEndpoints(context);
                });
            }).AddMassTransitHostedService();

            services.AddControllers();

            var identityServerHost = _configuration.GetValue<string>("Endpoints:IdentityServer");
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = identityServerHost;
                    options.Audience = "api1";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    OpenIdConnectUrl = new Uri(Path.Combine(identityServerHost, ".well-known/openid-configuration")),
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(Path.Combine(identityServerHost, "connect/authorize")),
                            TokenUrl = new Uri(Path.Combine(identityServerHost, "connect/token")),
                            Scopes = new Dictionary<string, string>
                            {
                                { "api1", "My api" }
                            }
                        }
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
                    c.OAuthClientId("client");
                    c.OAuthAppName("TodoApi");
                    c.OAuthUsePkce();
                });
            }
            else
            {
                app.UseMiddleware<ExceptionMiddleware>();
            }

            app.UseMiddleware<AppExceptionMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}