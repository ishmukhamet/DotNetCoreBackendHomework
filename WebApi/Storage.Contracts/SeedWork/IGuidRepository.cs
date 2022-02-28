using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Storage.Contracts.SeedWork
{
    public interface IGuidRepository<TEntity>
        where TEntity : class, IHaveId<Guid>
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity?> GetAsync(Guid id);

        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);
    }
}
