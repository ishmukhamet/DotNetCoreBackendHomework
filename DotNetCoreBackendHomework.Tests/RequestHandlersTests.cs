using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.AddTodoItem;
using WebApi.BusinessLogic.Contracts.Exceptions;
using WebApi.BusinessLogic.RequestHandlers;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;
using Xunit;

namespace DotNetCoreBackendHomework.Tests
{
    public class RequestHandlersTests
    {
        [Fact]
        public void GetTodoItemRequestHandler_ReturnConcreteItem()
        {
            // Arrange
            var mock = new Mock<ITodoItemRepository>();
            mock.Setup(repo => repo.GetAsync(Guid.Parse("58bfcbdd-26e7-47e6-aac1-df1c53ebe3b7")))
                .ReturnsAsync(GetTestItems().Where(i => i.Id == Guid.Parse("58bfcbdd-26e7-47e6-aac1-df1c53ebe3b7")).FirstOrDefault());
            var handler = new GetTodoItemRequestHandler(mock.Object);

            // Act
            var result = handler.HandleAsync(Guid.Parse("58bfcbdd-26e7-47e6-aac1-df1c53ebe3b7")).Result;

            // Assert
            Assert.Equal(GetTestItems().Where(i => i.Id == Guid.Parse("58bfcbdd-26e7-47e6-aac1-df1c53ebe3b7")).FirstOrDefault().Id, result.Id);
            Assert.Equal(GetTestItems().Where(i => i.Id == Guid.Parse("58bfcbdd-26e7-47e6-aac1-df1c53ebe3b7")).FirstOrDefault().Title, result.Title);
            Assert.Equal(GetTestItems().Where(i => i.Id == Guid.Parse("58bfcbdd-26e7-47e6-aac1-df1c53ebe3b7")).FirstOrDefault().IsCompleted, result.IsCompleted);
        }

        [Fact]
        public async Task GetTodoItemRequestHandler_ReturnNotFound()
        {
            // Arrange
            var mock = new Mock<ITodoItemRepository>();
            mock.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync((TodoItemEntity)null);
            var handler = new GetTodoItemRequestHandler(mock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(Guid.NewGuid()));

            // Assert
            Assert.Equal("NotFound", ex.ErrorCode);
        }

        [Fact]
        public void GetTodoItemListRequestHandler_ReturnAllItems()
        {
            // Arrange
            var mock = new Mock<ITodoItemRepository>();
            mock.Setup(repo => repo.GetAsync())
                .ReturnsAsync(GetTestItems());
            var handler = new GetTodoItemListRequestHandler(mock.Object);

            // Act
            var result = handler.HandleAsync().Result;

            // Assert
            Assert.Equal(GetTestItems().Count, result.Items.Count);
        }

        [Fact]
        public void AddTodoItemRequestHandler_GetNotEmptyGuid()
        {
            // Arrange
            var mock = new Mock<ITodoItemRepository>();
            mock.Setup(repo => repo.AddOrUpdateAsync(It.IsAny<TodoItemEntity>()))
                .ReturnsAsync(Guid.NewGuid());
            var handler = new AddTodoItemRequestHandler(mock.Object);

            // Act
            var result = handler.HandleAsync(new AddTodoItemRequest() { Title = "Testing of insert item" }).Result;

            // Assert
            Assert.True(Guid.Empty != result.Id);
        }

        private List<TodoItemEntity> GetTestItems()
        {
            var items = new List<TodoItemEntity>
            {
                new TodoItemEntity { Id=Guid.Parse("58bfcbdd-26e7-47e6-aac1-df1c53ebe3b7"), Title="Tom", IsCompleted=true},
                new TodoItemEntity { Id=Guid.Parse("b46cfc2a-1ae1-4cf8-a2db-509ed0d61314"), Title="Alice", IsCompleted=false},
                new TodoItemEntity { Id=Guid.Parse("9d3cbc9d-87a6-40df-8d10-d812f1b760c6"), Title="Sam", IsCompleted=true},
                new TodoItemEntity { Id=Guid.Parse("877144fc-5f3a-4f43-85ef-7600bc5b1834"), Title="Kate", IsCompleted=false}
            };
            return items;
        }
    }
}
