using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BusinessLogic.RequestHandlers;
using WebApi.Storage.Contracts.Repositories;
using Xunit;

namespace WebApi.Tests.RequestHandlers
{
    public class AddTodoItemTest
    {
        [Fact]
        public async Task ShouldCreateNewId()
        {
            var mock = new Mock<ITodoItemRepository>();
            var req = new BusinessLogic.Contracts.AddTodoItem.AddTodoItemRequest { Title = "hoba" };

            var handler = new AddTodoItemRequestHandler(mock.Object);
            var resp = await handler.HandleAsync(req);
            Assert.NotEqual(default(Guid), resp.Id);
        }
    }
}
