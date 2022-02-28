using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.Storage.Repositories
{
    public class TodoItemRepository : GuidRepository<TodoItemEntity>, ITodoItemRepository
    {
        public TodoItemRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}