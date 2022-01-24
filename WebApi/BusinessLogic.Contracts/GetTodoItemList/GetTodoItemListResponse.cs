using System;
using System.Collections.Generic;

namespace WebApi.BusinessLogic.Contracts.GetTodoItemList
{
    public class GetTodoItemListResponse
    {
        public List<GetTodoItemListElement> Items { get; set; } = new();
    }

    public class GetTodoItemListElement
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
    }
}