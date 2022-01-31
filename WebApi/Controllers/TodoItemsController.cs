using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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
        private readonly GetTodoItemListRequestHandler _getTodoItemListRequestHandler;
        private readonly AddTodoItemRequestHandler _addTodoItemRequestHandler;
        private readonly UpdateTodoItemRequestHandler _updateTodoItemRequestHandler;
        //private readonly IPublishEndpoint _publishEndpoint;

        public TodoItemsController(
            GetTodoItemRequestHandler getTodoItemRequestHandler,
            GetTodoItemListRequestHandler getTodoItemListRequestHandler,
            AddTodoItemRequestHandler addTodoItemRequestHandler,
            UpdateTodoItemRequestHandler updateTodoItemRequestHandler/*,
            IPublishEndpoint publishEndpoint*/
        )
        {
            _getTodoItemRequestHandler = getTodoItemRequestHandler;
            _getTodoItemListRequestHandler = getTodoItemListRequestHandler;
            _addTodoItemRequestHandler = addTodoItemRequestHandler;
            _updateTodoItemRequestHandler = updateTodoItemRequestHandler;
            //_publishEndpoint = publishEndpoint;
        }

        [HttpGet("{id:guid}")]
        public Task<GetTodoItemResponse> GetTodoItemAsync(Guid id)
        {
            return _getTodoItemRequestHandler.HandleAsync(id);
        }

        [HttpGet]
        public Task<GetTodoItemListResponse> GetTodoItemAsync()
        {
            return _getTodoItemListRequestHandler.HandleAsync();
        }

        [HttpPost]
        public Task<AddTodoItemResponse> AddTodoItemAsync([FromBody] AddTodoItemRequest request)
        {
            return _addTodoItemRequestHandler.HandleAsync(request);
        }

        [HttpPut("{id:guid}")]
        public async Task UpdateTodoItemAsync(Guid id, [FromBody] UpdateTodoItemRequest request)
        {
            //await _publishEndpoint.Publish<UpdateTodoItemMessage>(new
            //{
            //    id = id,
            //    title = request.Title,
            //    iscompleted = request.IsCompleted
            //});

            //return Ok();

            _ = _updateTodoItemRequestHandler.HandleAsync(id, request);
        }
    }
}