using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
            var item = await _todoItemRepository.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new GetTodoItemResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    IsCompleted = x.IsCompleted
                })
                .FirstOrDefaultAsync();

            if (item == null)
                throw new NotFoundException("Задача не найдена");

            return item;
        }
    }
}