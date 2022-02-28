using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.GetTodoItemList;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.BusinessLogic.RequestHandlers
{
    public class GetTodoItemListRequestHandler
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public GetTodoItemListRequestHandler(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        public async Task<GetTodoItemListResponse> HandleAsync()
        {
            var items = await _todoItemRepository.GetAll()
                .Select(x => new GetTodoItemListElement
                {
                    Id = x.Id,
                    Title = x.Title,
                    IsCompleted = x.IsCompleted
                })
                .ToListAsync();

            return new GetTodoItemListResponse
            {
                Items = items
            };
        }
    }
}
