using System;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.AddTodoItem;
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
            TodoItemEntity todoItemEntity = new TodoItemEntity();

            todoItemEntity.Title = request.Title;

            var itemId = await _todoItemRepository.AddOrUpdateAsync(todoItemEntity);

            if (itemId == Guid.Empty)
            {
                throw new Exception();
            }

            return new AddTodoItemResponse
            {
                Id = (Guid)itemId
            };
        }
    }
}