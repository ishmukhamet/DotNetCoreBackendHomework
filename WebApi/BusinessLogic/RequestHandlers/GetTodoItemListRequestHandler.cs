using AutoMapper;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.GetTodoItemList;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.BusinessLogic.RequestHandlers
{
    public class GetTodoItemListRequestHandler
    {
        private readonly ITodoItemRepository _repository;
        private readonly IMapper _mapper;

        public GetTodoItemListRequestHandler(ITodoItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetTodoItemListResponse> HandleAsync()
        {
            var result = new GetTodoItemListResponse()
            {
                Items = new System.Collections.Generic.List<GetTodoItemListElement>()
            };
            var items = await _repository.GetAllAsync();

            foreach (var item in items)
                result.Items.Add(_mapper.Map<GetTodoItemListElement>(item));

            return result;
        }
    }
}
