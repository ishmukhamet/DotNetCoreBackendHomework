using Microsoft.EntityFrameworkCore;
using WebApi.Storage.Contracts.Entities;

namespace WebApi.Storage
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TodoItemEntity> TodoItems { get; set; }
    }
}
