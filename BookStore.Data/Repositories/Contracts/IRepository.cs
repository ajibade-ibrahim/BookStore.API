using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Data.Repositories.Contracts
{
    public interface IRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        void Add(T entity);
        void Update(T entity);
        Task Delete(Guid id);
        void Delete(T entity);
        Task<bool> SaveAsync();
    }
}