using System;

namespace WebApi.Queue.Contracts
{
    public class UpdateTodoItemMessage
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
    }
}