using System.Collections.Generic;

namespace Code9Xamarin.Core.Mappers.Interfaces
{
    public interface IMapper<TSource, TDestination>
    {
        List<TDestination> ToDomainEntities(IEnumerable<TSource> source);
        TDestination ToDomainEntity(TSource source);
    }
}
