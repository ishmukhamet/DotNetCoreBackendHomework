using MassTransit;
using System.Threading.Tasks;
using WebApi.Queue.Contracts;

namespace WebApi.Queue
{
    public class UpdateTodoITemMessageConsumer : IConsumer<UpdateTodoItemMessage>
    {

        public async Task Consume(ConsumeContext<UpdateTodoItemMessage> context)
        {
            var id = context.Message.Id;
            var title = context.Message.Title;
            var isCompleted = context.Message.IsCompleted;
        }
    }
}