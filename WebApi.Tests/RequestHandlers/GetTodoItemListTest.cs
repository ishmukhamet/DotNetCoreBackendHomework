using MockQueryable.Moq;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApi.BusinessLogic.RequestHandlers;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;
using Xunit;

namespace WebApi.Tests.RequestHandlers
{
    public class GetTodoItemListTest
    {
        [Fact]
        public async Task ShouldReturnEmptyList()
        {
            var itemsMock = Array.Empty<TodoItemEntity>().AsQueryable().BuildMock();
            var mock = new Mock<ITodoItemRepository>();
            mock.Setup(repo => repo.GetAll()).Returns(itemsMock.Object);

            var handler = new GetTodoItemListRequestHandler(mock.Object);
            var resp = await handler.HandleAsync();

            Assert.NotNull(resp?.Items);
            Assert.Empty(resp.Items);
        }
    }
}
