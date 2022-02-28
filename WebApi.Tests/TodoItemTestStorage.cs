using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Storage.Contracts.Entities;

namespace WebApi.Tests
{
    class TodoItemTestStorage
    {
        public static List<TodoItemEntity> TodoItems = new()
        {
            new TodoItemEntity { Id = new Guid("{31A483E1-3C18-49CA-83E7-964E7B46C81D}"), Title = "abs", IsCompleted = false },
            new TodoItemEntity { Id = new Guid("{51001E16-CE68-4C92-B020-18547C08DF78}"), Title = "abs1", IsCompleted = true },
            new TodoItemEntity { Id = new Guid("{A48175D9-919A-4624-AA5B-9CF3313060A4}"), Title = "abs2", IsCompleted = false }
        };
    }
}
