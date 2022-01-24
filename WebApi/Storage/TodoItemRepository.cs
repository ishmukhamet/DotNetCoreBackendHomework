using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using WebApi.Storage.Contracts.Entities;
using WebApi.Storage.Contracts.Repositories;

namespace WebApi.Storage
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly string _connectionString;

        public TodoItemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("postgres");
        }

        public async Task<TodoItemEntity?> GetAsync(Guid id)
        {
            using var dbConnection = GetDbConnection();

            var item = await dbConnection.QueryFirstOrDefaultAsync<TodoItemEntity>(@"
                select * from todoItems
                where id = :id;
            ", new { id });

            return item;
        }

        public Task AddOrUpdateAsync(TodoItemEntity entity)
        {
            // TODO: implement
            throw new NotImplementedException();
        }

        private IDbConnection GetDbConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}