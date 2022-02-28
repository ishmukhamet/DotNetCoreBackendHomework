using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.BusinessLogic.Contracts.AddTodoItem;
using WebApi.BusinessLogic.Contracts.GetTodoItem;
using WebApi.BusinessLogic.Contracts.GetTodoItemList;
using WebApi.BusinessLogic.Contracts.UpdateTodoItem;
using WebApi.BusinessLogic.RequestHandlers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("todoItems")]
    public class TodoItemsController : ControllerBase
    {
        private readonly GetTodoItemRequestHandler _getTodoItemRequestHandler;
        private readonly AddTodoItemRequestHandler _addTodoItemRequestHandler;
        private readonly UpdateTodoItemRequestHandler _updateTodoItemRequestHandler;
        private readonly GetTodoItemListRequestHandler _getTodoItemListRequestHandler;

        public TodoItemsController(
            GetTodoItemRequestHandler getTodoItemRequestHandler,
            AddTodoItemRequestHandler addTodoItemRequestHandler,
            UpdateTodoItemRequestHandler updateTodoItemRequestHandler,
            GetTodoItemListRequestHandler getTodoItemListRequestHandler
        )
        {
            _getTodoItemRequestHandler = getTodoItemRequestHandler;
            _addTodoItemRequestHandler = addTodoItemRequestHandler;
            _updateTodoItemRequestHandler = updateTodoItemRequestHandler;
            _getTodoItemListRequestHandler = getTodoItemListRequestHandler;
        }

        [HttpGet]
        public Task<GetTodoItemListResponse> GetTodoItemListAsync()
        {
            return _getTodoItemListRequestHandler.HandleAsync();
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public Task<GetTodoItemResponse> GetTodoItemAsync(Guid id)
        {
            return _getTodoItemRequestHandler.HandleAsync(id);
        }

        [HttpPost]
        [Authorize]
        public Task<AddTodoItemResponse> AddTodoItemAsync([FromBody] AddTodoItemRequest request)
        {
            return _addTodoItemRequestHandler.HandleAsync(request);
        }

        [HttpPut("{id:guid}")]
        public Task UpdateTodoItemAsync(Guid id, [FromBody] UpdateTodoItemRequest request)
        {
            return _updateTodoItemRequestHandler.HandleAsync(id, request);
        }
    }
}