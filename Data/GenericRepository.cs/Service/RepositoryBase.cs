using HD_Backend.Data.GenericRepository.cs.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HD_Backend.Data.GenericRepository.cs.Service;
   
        public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
        {
            protected HelpDeskDbContext HelpDeskDbContext;
            public RepositoryBase(HelpDeskDbContext helpDeskDbContext) =>
                HelpDeskDbContext = helpDeskDbContext;

            public async Task<IQueryable<T>> FindAllAsync(bool trackChanges) =>
                !trackChanges ? await Task.Run(() => HelpDeskDbContext.Set<T>().AsNoTracking()) : await Task.Run(() => HelpDeskDbContext.Set<T>());

            public async Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges) =>
                !trackChanges ? await Task.Run(() => HelpDeskDbContext.Set<T>().Where(expression).AsNoTracking()) : await Task.Run(() => HelpDeskDbContext.Set<T>().Where(expression));

            public async Task CreateAsync(T entity) => await Task.Run(() => HelpDeskDbContext.Set<T>().Add(entity));

            public async Task UpdateAsync(T entity) => await Task.Run(() => HelpDeskDbContext.Set<T>().Update(entity));
            public async Task RemoveAsync(T entity) => await Task.Run(() => HelpDeskDbContext.Set<T>().Remove(entity));
        }
  
