using System;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.UpdateTodoItem;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.BusinessLogic.RequestHandlers
{
    public class UpdateTodoItemRequestHandler
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public UpdateTodoItemRequestHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }
        public async Task HandleAsync(Guid id, UpdateTodoItemRequest request)
        {
            TodoItemEntity todoItemEntity = new TodoItemEntity();

            todoItemEntity.Id = id;
            todoItemEntity.Title = request.Title;
            todoItemEntity.IsCompleted = request.IsCompleted;

            var itemId = await _todoItemRepository.AddOrUpdateAsync(todoItemEntity);
        }
    }
}