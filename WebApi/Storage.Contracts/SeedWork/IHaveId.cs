using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Storage.Contracts.SeedWork
{
    public interface IHaveId<TId>
    {
        public TId Id { get; set; }
    }
}
