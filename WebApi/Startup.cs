using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebApi.BusinessLogic.RequestHandlers;
using WebApi.Engine;
using WebApi.Queue;
using WebApi.Storage;
using WebApi.Storage.Contracts.Repositories;
using static WebApi.Engine.ErrorFilter;

namespace WebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Обработчики запросов
            services.AddScoped<AddTodoItemRequestHandler>();
            services.AddScoped<UpdateTodoItemRequestHandler>();
            services.AddScoped<GetTodoItemRequestHandler>();
            services.AddScoped<GetTodoItemListRequestHandler>();

            // Хранилища
            services.AddScoped<ITodoItemRepository, TodoItemRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" }); });
            services.AddMvc(options => options.Filters.Add(typeof(ErrorFilter))).SetCompatibilityVersion(CompatibilityVersion.Latest);

            //обработка ошибки во время привязки модели
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ErrorData
                    (
                        "Некорректная модель запроса",
                        "InvalidModel"
                    );

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });

            //services.AddMassTransit(x =>
            //{
            //    x.AddConsumer<UpdateTodoITemMessageConsumer>();

            //    x.UsingRabbitMq((context, cfg) =>
            //    {
            //        cfg.ReceiveEndpoint("event-listener", e =>
            //        {
            //            e.ConfigureConsumer<UpdateTodoITemMessageConsumer>(context);
            //        });
            //    });
            //});

            //services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}