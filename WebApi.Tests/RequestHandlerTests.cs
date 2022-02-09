using AutoMapper;
using MassTransit.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.AddTodoItem;
using WebApi.BusinessLogic.Contracts.Exceptions;
using WebApi.BusinessLogic.Contracts.UpdateTodoItem;
using WebApi.BusinessLogic.RequestHandlers;
using WebApi.Engine.Mapper;
using WebApi.Queue.Contracts;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;
using WebApi.Tests.Mocks;
using Xunit;

namespace WebApi.Tests
{
    public class RequestHandlerTests
    {

        IMapper mapper;

        public RequestHandlerTests()
        {
            MapperConfiguration mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            mapper = mockMapper.CreateMapper();
        }

        [Fact]
        public async Task GetTodoItemListRequestHandler_ReturnsAll()
        {
            // Arrange
            var items = new List<TodoItemEntity>()
            {
                new TodoItemEntity(){ Id=new Guid("796848D9-F03C-4B44-9C80-870EAACABDE3"), IsCompleted=false, Title="Задание1"},
                new TodoItemEntity(){ Id=new Guid("663190F2-1123-473F-9C52-49A8F4AF38DB"), IsCompleted=true, Title="Задание2"},
                new TodoItemEntity(){ Id=new Guid("C64E3220-92E0-4283-A915-0804E2F50671"), IsCompleted=false, Title="Задание3"}
            };

            var handler = new GetTodoItemListRequestHandler(new TodoItemRepositoryMock(items), mapper);

            // Act
            var result = await handler.HandleAsync();

            // Assert
            Assert.Equal(items.Count, result.Items.Count);
        }

        [Fact]
        public async Task GetTodoItemListRequestHandler_ReturnsEmptyArray()
        {
            // Arrange
            var handler = new GetTodoItemListRequestHandler(new TodoItemRepositoryMock(new List<TodoItemEntity>()), mapper);

            // Act
            var result = await handler.HandleAsync();

            // Assert
            Assert.NotNull(result.Items);
            Assert.Empty(result.Items);
        }

        [Fact]
        public async Task GetTodoItemRequestHandler_ReturnsElement()
        {
            // Arrange
            var guidToGet = new Guid("796848D9-F03C-4B44-9C80-870EAACABDE3");
            var item = new TodoItemEntity() { Id = guidToGet, Title = "Тест", IsCompleted = true };
            var items = new List<TodoItemEntity>()
            {
                item
            };
            var handler = new GetTodoItemRequestHandler(new TodoItemRepositoryMock(items), mapper);

            // Act
            var result = await handler.HandleAsync(guidToGet);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(guidToGet, result.Id);
            Assert.Equal(item.IsCompleted, result.IsCompleted);
            Assert.Equal(item.Title, result.Title);
        }

        [Fact]
        public async Task GetTodoItemRequestHandler_ReturnsNotFound()
        {
            // Arrange
            var guidToGet = new Guid("796848D9-F03C-4B44-9C80-870EAACABDE3");
            var guidNotFound = new Guid("85BA9DC8-9006-4A83-8C00-3A29E98A807C");
            var item = new TodoItemEntity() { Id = guidToGet, Title = "Тест", IsCompleted = true };
            var items = new List<TodoItemEntity>()
            {
                item
            };
            var handler = new GetTodoItemRequestHandler(new TodoItemRepositoryMock(items), mapper);

            // Act & Assert
            var exc = await Assert.ThrowsAsync<NotFoundException>(async () => await handler.HandleAsync(guidNotFound));
            Assert.Equal("NotFound", exc.ErrorCode);
        }

        [Fact]
        public async Task AddTodoItemRequestHandler_AddsItem()
        {
            // Arrange
            var items = new List<TodoItemEntity>();
            var handler = new AddTodoItemRequestHandler(new TodoItemRepositoryMock(items), mapper);
            var addItem = new AddTodoItemRequest() { Title = "Тест" };

            // Act
            var result = await handler.HandleAsync(addItem);

            // Assert
            Assert.IsType<Guid>(result.Id);
            Assert.NotEmpty(items);
            var item = items.LastOrDefault();
            Assert.Equal(addItem.Title, item.Title);
            Assert.False(item.IsCompleted);
        }

        [Fact]
        public async Task UpdateTodoItemRequestHandler_PushesAlert()
        {
            // Arrange
            var harness = new InMemoryTestHarness();
            var items = new List<TodoItemEntity>();
            await harness.Start();
            var handler = new UpdateTodoItemRequestHandler(new TodoItemRepositoryMock(items), mapper, harness.Bus);
            var updateItem = new UpdateTodoItemRequest() { Title = "Тест", IsCompleted = false };

            // Act & Assert
            try
            {
                await handler.HandleAsync(new Guid(), updateItem);
                Assert.True( await harness.Published.Any<UpdateTodoItemMessage>());
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
