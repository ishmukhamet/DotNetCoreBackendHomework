using MassTransit.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.UpdateTodoItem;
using WebApi.BusinessLogic.RequestHandlers;
using WebApi.Queue.Contracts;
using Xunit;

namespace WebApi.Tests.RequestHandlers
{
    public class UpdateTodoItemTest
    {
        [Fact]
        public async Task ShouldPublishUpdateMessage()
        {
            var harness = new InMemoryTestHarness();
            await harness.Start();

            var handler = new UpdateTodoItemRequestHandler(harness.Bus);
            var req = new UpdateTodoItemRequest() { Title = "Тест", IsCompleted = false };

            // Act & Assert
            try
            {
                await handler.HandleAsync(Guid.NewGuid(), req);
                Assert.True(await harness.Published.Any<UpdateTodoItemMessage>());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
