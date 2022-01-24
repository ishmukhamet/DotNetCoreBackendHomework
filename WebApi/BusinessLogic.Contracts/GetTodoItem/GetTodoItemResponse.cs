using System;

namespace WebApi.BusinessLogic.Contracts.GetTodoItem
{
    public class GetTodoItemResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
    }
}