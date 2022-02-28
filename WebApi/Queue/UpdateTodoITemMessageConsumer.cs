using MassTransit;
using System.Threading.Tasks;
using WebApi.Queue.Contracts;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.Queue
{
    public class UpdateTodoItemMessageConsumer : IConsumer<UpdateTodoItemMessage>
    {
        private readonly ITodoItemRepository _repository;

        public UpdateTodoItemMessageConsumer(ITodoItemRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<UpdateTodoItemMessage> context)
        {
            var msg = context.Message;
            var entity = await _repository.GetAsync(msg.Id);

            if(entity != null)
            {
                entity.Title = msg.Title;
                entity.IsCompleted = msg.IsCompleted;

                await _repository.UpdateAsync(entity);
            }
        }
    }
}