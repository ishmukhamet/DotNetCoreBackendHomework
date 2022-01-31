using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using WebApi.BusinessLogic.Contracts.AddTodoItem;
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

        public async Task<List<TodoItemEntity>?> GetAsync()
        {
            using var dbConnection = GetDbConnection();

            var item = await dbConnection.QueryAsync<TodoItemEntity>(@"
                select * from todoItems");

            return item.ToList();
        }

        public async Task<Guid?> AddOrUpdateAsync(TodoItemEntity entity)
        {
            using var dbConnection = GetDbConnection();

            if (entity.Id == Guid.Empty)
            {
                var itemId = await dbConnection.QuerySingleOrDefaultAsync<AddTodoItemResponse>(@"
                INSERT INTO public.todoitems(
	            id, title, ""isCompleted"")
                VALUES(uuid_generate_v4(), @title, @isCompleted)
                RETURNING id;
                ", new { title = entity.Title, isCompleted = entity.IsCompleted });

                return itemId.Id;
            }
            else
            {
                var itemId = await dbConnection.ExecuteAsync(@"
                UPDATE public.todoitems
	            SET title=@title, ""isCompleted""=@isCompleted
                WHERE id = @id
                ", new { id = entity.Id,title = entity.Title, isCompleted = entity.IsCompleted });

                return null;
            }
        }

        private IDbConnection GetDbConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}