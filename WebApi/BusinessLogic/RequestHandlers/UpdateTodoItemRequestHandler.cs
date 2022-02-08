using AutoMapper;
using System;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.UpdateTodoItem;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.BusinessLogic.RequestHandlers
{
    public class UpdateTodoItemRequestHandler
    {
        private readonly IMapper _mapper;
        private readonly ITodoItemRepository _repository;

        public UpdateTodoItemRequestHandler(ITodoItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task HandleAsync(Guid id, UpdateTodoItemRequest request)
        {
            var entity = _mapper.Map<TodoItemEntity>(request);
            entity.Id = id;
            return _repository.AddOrUpdateAsync(entity);
        }
    }
}