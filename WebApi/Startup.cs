using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebApi.BusinessLogic.Contracts.Exceptions;
using WebApi.BusinessLogic.RequestHandlers;
using MassTransit;
using WebApi.Engine.Filters;
using WebApi.Engine.Mapper;
using WebApi.Storage;
using WebApi.Storage.Contracts.Repositories;
using WebApi.Queue;
using WebApi.HostedServices;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi
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
            // Обработчики запросов
            services.AddScoped<AddTodoItemRequestHandler>();
            services.AddScoped<UpdateTodoItemRequestHandler>();
            services.AddScoped<GetTodoItemListRequestHandler>();
            services.AddScoped<GetTodoItemRequestHandler>();
            services.AddScoped<UpdateTodoITemMessageConsumer>();

            // Хранилища
            services.AddScoped<ITodoItemRepository, TodoItemRepository>();

            //Маппер
            services.AddAutoMapper(configAction: (config) => config.AddProfile<MappingProfile>());

            //Фильтр исключений
            services.AddMvc(options =>
            {
                options.Filters.Add<ErrorFilter>();
            });
            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = actionContext =>
                    throw new BadRequestException("InvalidModel");
            });

            //MassTransit
            var massTransitSection = Configuration.GetSection("MassTransit");
            var url = massTransitSection.GetValue<string>("Url");
            var userName = massTransitSection.GetValue<string>("UserName");
            var password = massTransitSection.GetValue<string>("Password");

            services.AddMassTransit(x =>
            {
                x.AddConsumer<UpdateTodoITemMessageConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(url, configurator =>
                    {
                        configurator.Username(userName);
                        configurator.Password(password);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });
            services.AddMassTransitHostedService(true);

            services.AddControllers();
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Audience = "todo-api";
                    options.Authority = "http://host.docker.internal:5003";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new
                    TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                    };
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
                c.OperationFilter<SwaggerAuthenticationRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    OpenIdConnectUrl = new Uri("http://localhost:5003/.well-known/openid-configuration"),
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("http://localhost:5003/connect/authorize"),
                            TokenUrl = new Uri("http://localhost:5003/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"todo-api", "Demo API - full access"}
                            }
                        },
                    }
                });
                ;
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
                    c.OAuthAppName("Todo API - Swagger");
                    c.OAuthUsePkce();
                });
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }

    public class SwaggerAuthenticationRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Security == null)
            {
                operation.Security = new List<OpenApiSecurityRequirement>();
            }

            var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" } };

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            });
        }
    }

}