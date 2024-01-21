using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<TEntity> where TEntity:BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecification<TEntity> specs)
        {
            var query = inputQuery;                       //context.Prouducts

            if(specs.Criteria != null)
                query = query.Where(specs.Criteria);      //context.Prouducts.Where(p=>p.Id==id)

            if(specs.OrderBy != null)
                query = query.OrderBy(specs.OrderBy);

            if(specs.OrderByDescending != null)
                query = query.OrderByDescending(specs.OrderByDescending);

            if(specs.IsPaginationEnabled)
                query=query.Skip(specs.Skip).Take(specs.Take);

            query = specs.Includes.Aggregate(query, (currentQuery, includes) => currentQuery.Include(includes));
            //context.Prouducts.Where(p=>p.Id==id).Include(p=>p.ProductBrand).Include(p=>p.ProductType);
            return query;
        }
    }
}
