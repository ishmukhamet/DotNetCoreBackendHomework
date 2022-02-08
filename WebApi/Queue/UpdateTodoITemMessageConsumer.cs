using AutoMapper;
using MassTransit;
using MassTransit.Definition;
using System.Threading.Tasks;
using WebApi.Queue.Contracts;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.Queue
{
    public class UpdateTodoITemMessageConsumer : IConsumer<UpdateTodoItemMessage>
    {
        private readonly ITodoItemRepository _repository;
        private readonly IMapper _mapper;

        public UpdateTodoITemMessageConsumer(ITodoItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task Consume(ConsumeContext<UpdateTodoItemMessage> context)
        {
            var entity = _mapper.Map<TodoItemEntity>(context.Message);
            return _repository.AddOrUpdateAsync(entity);
        }
    }
}