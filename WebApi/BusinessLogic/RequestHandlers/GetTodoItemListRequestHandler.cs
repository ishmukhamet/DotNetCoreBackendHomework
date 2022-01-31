using System;
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
            var items = await _todoItemRepository.GetAsync();

            if (items == null)
            {
                throw new Exception();
            }

            GetTodoItemListResponse todoItemListResponse = new GetTodoItemListResponse();

            foreach (var item in items)
            {
                GetTodoItemListElement todoItemListElement = new GetTodoItemListElement();

                todoItemListElement.Id = item.Id;
                todoItemListElement.Title = item.Title;
                todoItemListElement.IsCompleted = item.IsCompleted;

                todoItemListResponse.Items.Add(todoItemListElement);
            }

            return todoItemListResponse;
        }
    }
}