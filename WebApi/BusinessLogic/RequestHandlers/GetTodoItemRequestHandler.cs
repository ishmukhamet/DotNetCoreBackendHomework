using System;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.Exceptions;
using WebApi.BusinessLogic.Contracts.GetTodoItem;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.BusinessLogic.RequestHandlers
{
    public class GetTodoItemRequestHandler
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public GetTodoItemRequestHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        public async Task<GetTodoItemResponse> HandleAsync(Guid id)
        {
            var item = await _todoItemRepository.GetAsync(id);

            if (item == null)
            {
                throw new BadRequestException("NotFound");
            }

            return new GetTodoItemResponse
            {
                Id = item.Id,
                Title = item.Title,
                IsCompleted = item.IsCompleted
            };
        }
    }
}