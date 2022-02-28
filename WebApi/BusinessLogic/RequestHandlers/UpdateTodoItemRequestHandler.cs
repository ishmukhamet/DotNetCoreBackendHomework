using MassTransit;
using System;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.UpdateTodoItem;
using WebApi.Queue.Contracts;

namespace WebApi.BusinessLogic.RequestHandlers
{
    public class UpdateTodoItemRequestHandler
    {
        private readonly IBus _bus;

        public UpdateTodoItemRequestHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task HandleAsync(Guid id, UpdateTodoItemRequest request)
        {
            await _bus.Publish(new UpdateTodoItemMessage
            {
                Id = id,
                IsCompleted = request.IsCompleted,
                Title = request.Title
            });
        }
    }
}