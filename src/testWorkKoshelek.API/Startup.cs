using testWorkKoshelek.API.Extensions;
using Microsoft.OpenApi.Models;
using testWorkKoshelek.API.Maps;
using Infrastructure.WebSockets.Maps;

namespace testWorkKoshelek.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Koshelek.ru API",
                    Description = "Тестовое задание для компании кошелек.ру",
                    Contact = new OpenApiContact
                    {
                        Name = "Андрей Мисирук",
                        Url = new Uri("https://t.me/holydreams117")
                    }
                });

                var basePath = AppContext.BaseDirectory;

                var xmlPath = Path.Combine(basePath, "testWorkKoshelek.API.xml");
                options.IncludeXmlComments(xmlPath);
            });

            services.AddInfrastructure(Configuration);
            services.AddBase();
            services.AddLogic();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MessageDomainToDto>();
                cfg.AddProfile<MessageDomainToMessageRecieveDTO>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next.Invoke();
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "testWorkKoshelekApi");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseWebSockets();
        }
    }
}
