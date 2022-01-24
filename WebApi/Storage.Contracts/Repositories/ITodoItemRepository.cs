using System;
using System.Threading.Tasks;
using WebApi.Storage.Contracts.Entities;

namespace WebApi.Storage.Contracts.Repositories
{
    public interface ITodoItemRepository
    {
        Task<TodoItemEntity?> GetAsync(Guid id);
        Task AddOrUpdateAsync(TodoItemEntity entity);
    }
}