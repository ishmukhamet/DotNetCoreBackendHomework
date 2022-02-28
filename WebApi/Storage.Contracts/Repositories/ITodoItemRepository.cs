using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.SeedWork;

namespace WebApi.Storage.Contracts.Repositories
{
    public interface ITodoItemRepository : IGuidRepository<TodoItemEntity>
    {
    }
}