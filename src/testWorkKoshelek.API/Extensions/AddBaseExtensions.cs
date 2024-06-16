using Base.Infrastructure.AutoMapper;
using Base.Interfaces;

namespace testWorkKoshelek.API.Extensions
{
    public static class AddBaseExtensions
    {
        public static IServiceCollection AddBase(this IServiceCollection services)
        {
            services.AddScoped<IAutoMapperAdapter, AutoMapperAdapter>();

            return services;
        }
    }
}
