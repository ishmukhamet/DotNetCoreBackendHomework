using System;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.AddTodoItem;
using WebApi.Engine;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.BusinessLogic.RequestHandlers
{
    public class AddTodoItemRequestHandler
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public AddTodoItemRequestHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        public async Task<AddTodoItemResponse> HandleAsync(AddTodoItemRequest request)
        {
            var entity = new TodoItemEntity
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                IsCompleted = false
            };

            await _todoItemRepository.AddAsync(entity);
            return new AddTodoItemResponse { Id = entity.Id };
        }
    }
}