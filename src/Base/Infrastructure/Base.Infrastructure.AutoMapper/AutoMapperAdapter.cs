using AutoMapper;
using Base.Interfaces;
using System.Linq.Expressions;

namespace Base.Infrastructure.AutoMapper
{
    public class AutoMapperAdapter : IAutoMapperAdapter
    {
        private readonly IMapper _mappingEngine;

        public AutoMapperAdapter(IMapper mappingEngine)
        {
            _mappingEngine = mappingEngine;
        }

        public TDestination Map<TDestination>(object source)
        {
            return _mappingEngine.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mappingEngine.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mappingEngine.Map(source, destination);
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return _mappingEngine.Map(source, sourceType, destinationType);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return _mappingEngine.Map(source, destination, sourceType, destinationType);
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, object? parameters = null, params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            return _mappingEngine.ProjectTo<TDestination>(source, parameters, membersToExpand);
        }

        public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, IDictionary<string, object?> parameters, params string[] membersToExpand)
        {
            return _mappingEngine.ProjectTo<TDestination>(source, parameters, membersToExpand);
        }
    }
}
