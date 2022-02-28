using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using WebApi.Storage.Contracts.SeedWork;

namespace WebApi.Storage.Contracts.Entities
{
    public class TodoItemEntity : IHaveId<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public bool IsCompleted { get; set; }
    }
}