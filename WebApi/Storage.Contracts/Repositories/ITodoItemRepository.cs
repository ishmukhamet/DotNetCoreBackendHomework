using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Storage.Contracts.Entities;

namespace WebApi.Storage.Contracts.Repositories
{
    public interface ITodoItemRepository
    {
        Task<TodoItemEntity?> GetAsync(Guid id);
        Task<List<TodoItemEntity>?> GetAsync();
        Task<Guid?> AddOrUpdateAsync(TodoItemEntity entity);
    }
}