using System;

namespace WebApi.Storage.Contracts.Entities
{
    public class TodoItemEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; } = false;
    }
}