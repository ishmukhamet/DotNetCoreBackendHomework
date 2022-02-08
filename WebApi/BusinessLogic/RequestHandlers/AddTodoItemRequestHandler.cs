using AutoMapper;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.AddTodoItem;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.BusinessLogic.RequestHandlers
{
    public class AddTodoItemRequestHandler
    {
        private readonly ITodoItemRepository _repository;
        private readonly IMapper _mapper;

        public AddTodoItemRequestHandler(ITodoItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<AddTodoItemResponse> HandleAsync(AddTodoItemRequest request)
        {
            var entity = _mapper.Map<TodoItemEntity>(request);
            var result  = await _repository.AddOrUpdateAsync(entity);

            return new AddTodoItemResponse()
            {
                Id = result
            };
        }
    }
}