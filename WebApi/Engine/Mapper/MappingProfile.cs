using AutoMapper;
using WebApi.BusinessLogic.Contracts.AddTodoItem;
using WebApi.BusinessLogic.Contracts.GetTodoItem;
using WebApi.BusinessLogic.Contracts.GetTodoItemList;
using WebApi.BusinessLogic.Contracts.UpdateTodoItem;
using WebApi.Storage.Contracts.Entities;

namespace WebApi.Engine.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoItemEntity, GetTodoItemListElement>();
            CreateMap<GetTodoItemListElement, TodoItemEntity>();

            CreateMap<TodoItemEntity, AddTodoItemRequest>();
            CreateMap<AddTodoItemRequest, TodoItemEntity>();

            CreateMap<TodoItemEntity, UpdateTodoItemRequest>();
            CreateMap<UpdateTodoItemRequest, TodoItemEntity>();

            CreateMap<GetTodoItemResponse, TodoItemEntity>();
            CreateMap<TodoItemEntity, GetTodoItemResponse>();
        }
    }
}
