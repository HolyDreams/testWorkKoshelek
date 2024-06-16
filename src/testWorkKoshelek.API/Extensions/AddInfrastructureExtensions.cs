using Core.Domain.Models.Settings;
using Core.Interfaces.DataAccess;
using Core.Interfaces.WebSocketConnection;
using Infrastructure.DataAccess;
using Infrastructure.WebSockets;
using Microsoft.Extensions.Options;
using WebSocketOptions = Core.Domain.Models.Settings.WebSocketOptions;

namespace testWorkKoshelek.API.Extensions
{
    public static class AddInfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<DbOptions>(configuration.GetSection("DbOptions"))
                .Configure<WebSocketOptions>(configuration.GetSection("WebSocketOptions"));

            var provider = services.BuildServiceProvider();

            if (provider.GetService<IOptions<DbOptions>>() is null)
            {
                throw new Exception("Отсутствуют насройки базы данных");
            }
            if (provider.GetService<IOptions<WebSocketOptions>>() is null)
            {
                throw new Exception("Отсутсвуют настройки веб сокета");
            }

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IWebSocketStreamProvider, WebSocketStreamProvider>();

            return services;
        }
    }
}
