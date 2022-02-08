using AutoMapper;
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
        private readonly IMapper _mapper;

        public GetTodoItemRequestHandler(ITodoItemRepository todoItemRepository, IMapper mapper)
        {
            _todoItemRepository = todoItemRepository;
            _mapper = mapper;
        }

        public async Task<GetTodoItemResponse> HandleAsync(Guid id)
        {
            var item = await _todoItemRepository.GetAsync(id);

            if (item == null)
            {
                throw new NotFoundException("NotFound");
            }
            return _mapper.Map<GetTodoItemResponse>(item);
        }
    }
}