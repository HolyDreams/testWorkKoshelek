using System.Linq.Expressions;

namespace Base.Interfaces
{
    public interface IAutoMapperAdapter
    {
        TDestination Map<TDestination>(object source);

        TDestination Map<TSource, TDestination>(TSource source);

        TDestination Map<TSource, TDestination>(TSource source, TDestination destination);

        object Map(object source, Type sourceType, Type destinationType);

        object Map(object source, object destination, Type sourceType, Type destinationType);

        IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, object? parameters = null,
            params Expression<Func<TDestination, object>>[] membersToExpand);

        IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source, IDictionary<string, object?> parameters,
            params string[] membersToExpand);
    }
}
