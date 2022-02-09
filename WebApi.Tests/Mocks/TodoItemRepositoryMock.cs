using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.Tests.Mocks
{
    internal class TodoItemRepositoryMock : ITodoItemRepository
    {
        private List<TodoItemEntity> _items;

        public TodoItemRepositoryMock(List<TodoItemEntity> items)
        {
            _items = items;
        }


        public Task<Guid> AddOrUpdateAsync(TodoItemEntity entity)
        {
            if (entity.Id == Guid.Empty)
            {
                var newItem = new TodoItemEntity() { Id = Guid.NewGuid(), IsCompleted = entity.IsCompleted, Title = entity.Title };
                _items.Add(newItem);
                return Task.FromResult(newItem.Id);
            }

            var item = _items.SingleOrDefault(t => t.Id == entity.Id);
            item.IsCompleted = entity.IsCompleted;
            item.Title = entity.Title;
            return Task.FromResult(Guid.Empty);
        }

        public Task<IEnumerable<TodoItemEntity>> GetAllAsync()
        {
            return Task.FromResult(_items.AsEnumerable());
        }

        public Task<TodoItemEntity> GetAsync(Guid id)
        {
            return Task.FromResult(_items.SingleOrDefault(t => t.Id == id));
        }
    }
}
