using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using WebApi.BusinessLogic.Contracts.Exceptions;
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

        public async Task<IEnumerable<TodoItemEntity>> GetAllAsync()
        {
            using var dbConnection = GetDbConnection();

            var items = await dbConnection.QueryAsync<TodoItemEntity>(@"
                select * FROM public.""todoItems""");
            return items;
        }

        public async Task<TodoItemEntity?> GetAsync(Guid id)
        {
            using var dbConnection = GetDbConnection();

            var item = await dbConnection.QueryFirstOrDefaultAsync<TodoItemEntity>(@"
                select * FROM public.""todoItems""
                where id = :id;
            ", new { id });

            return item;
        }

        public async Task<Guid> AddOrUpdateAsync(TodoItemEntity entity)
        {
            using var dbConnection = GetDbConnection();

            if (entity.Id == Guid.Empty)
            {
                var result = await dbConnection.ExecuteScalarAsync<Guid>(@"
                    INSERT INTO public.""todoItems""(id, title, ""isCompleted"")
                    VALUES (uuid_generate_v4(), :title, :isCompleted)
                    RETURNING id
                ", entity);

                return result;
            }

            //update
            var rowsAffected = await dbConnection.ExecuteAsync(@"
                UPDATE public.""todoItems""
	            SET title=:title, ""isCompleted""=:isCompleted
                WHERE id = @id
                ", entity);

            if (rowsAffected == 0)
                throw new NotFoundException("NotFound");
            return entity.Id;
        }

        private IDbConnection GetDbConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}