using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Identity.Data;

namespace Talabat.Repository
{
    public class UniteOfWork : IUniteOfWork
    {
        private readonly StoreContext context;
        private Hashtable _repositories;

        public UniteOfWork(StoreContext context)
        {
            this.context = context;
            _repositories = new Hashtable();
        }
        public  IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            
            if (!_repositories.ContainsKey(type))
            {
                var repository =  new GenericRepository<TEntity>(context);

                 _repositories.Add(type,repository);
            }
            return  _repositories[type] as IGenericRepository<TEntity>;
        }
        public async Task<int> Complete()
        {
            return await context.SaveChangesAsync();
        }

        public ValueTask DisposeAsync()
        {
            return context.DisposeAsync();
        }

    }
}
