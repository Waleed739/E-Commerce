using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Identity.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            this.dbContext = dbContext;
        }


        #region StaticQuery

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if (typeof(T) == typeof(Product))
            //    return (IEnumerable<T>) await dbContext.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync(); 
            return await dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int Id)
         => await dbContext.Set<T>().FindAsync(Id);
        #endregion


        #region DynamicQuery

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecificaion(spec).ToListAsync();
        }
        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecificaion(spec).CountAsync();
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecificaion(spec).FirstOrDefaultAsync();
        }
        public IQueryable<T> ApplySpecificaion (ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(dbContext.Set<T>(), spec);
        }

        public async Task Add(T entity)
        => await dbContext.Set<T>().AddAsync(entity);

        public void Update(T entity)
         =>  dbContext.Set<T>().Update(entity);


        public void Delete(T entity)
        => dbContext.Set<T>().Remove(entity);



        #endregion
    }
}
