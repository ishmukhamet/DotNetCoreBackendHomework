using Moq;
using MockQueryable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.Exceptions;
using WebApi.BusinessLogic.RequestHandlers;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;
using Xunit;
using MockQueryable.Moq;

namespace WebApi.Tests.RequestHandlers
{
    public class GetTodoItemTest
    {
        [Fact]
        public async Task ShouldThrowNotFoundException()
        {
            var itemsMock = Array.Empty<TodoItemEntity>().AsQueryable().BuildMock();

            var mock = new Mock<ITodoItemRepository>();
            mock.Setup(repo => repo.GetAll()).Returns(itemsMock.Object);

            var handler = new GetTodoItemRequestHandler(mock.Object);

            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.HandleAsync(Guid.Empty));
        }

        [Fact]
        public async Task ShouldReturnItem()
        {
            var itemsMock = TodoItemTestStorage.TodoItems.AsQueryable().BuildMock();
            var mock = new Mock<ITodoItemRepository>();

            mock.Setup(repo => repo.GetAll()).Returns(itemsMock.Object);

            var handler = new GetTodoItemRequestHandler(mock.Object);

            var testItem = TodoItemTestStorage.TodoItems[0];
            var resp = await handler.HandleAsync(testItem.Id);

            Assert.NotNull(resp);
            Assert.Equal(testItem.Id, resp.Id);
            Assert.Equal(testItem.Title, resp.Title);
            Assert.Equal(testItem.IsCompleted, resp.IsCompleted);
        }
    }
}
