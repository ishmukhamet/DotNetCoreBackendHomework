using AutoMapper;
using MassTransit;
using System;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.UpdateTodoItem;
using WebApi.Queue.Contracts;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.BusinessLogic.RequestHandlers
{
    public class UpdateTodoItemRequestHandler
    {
        private readonly IMapper _mapper;
        private readonly ITodoItemRepository _repository;
        private readonly IBus _bus;

        public UpdateTodoItemRequestHandler(ITodoItemRepository repository, IMapper mapper, IBus bus)
        {
            _repository = repository;
            _mapper = mapper;
            _bus = bus;
        }

        public Task HandleAsync(Guid id, UpdateTodoItemRequest request)
        {
            var message = _mapper.Map<UpdateTodoItemMessage>(request);
            message.Id = id;
            return _bus.Publish(message);
        }
    }
}