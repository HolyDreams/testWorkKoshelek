using Logic;
using Logic.Interfaces;

namespace testWorkKoshelek.API.Extensions
{
    public static class AddLogicExtensions
    {
        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            services.AddScoped<IGetterMessage, GetterMessage>();
            services.AddScoped<ISenderMessage, SenderMessage>();

            return services;
        }
    }
}
