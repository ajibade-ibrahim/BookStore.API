using System;
using System.Threading.Tasks;
using BookStore.Domain.Entities;

namespace BookStore.Data.Repositories.Contracts
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task CreateAuthorAsync(Author author);
        Task UpdateAuthorAsync(Author author);
        Task DeleteAuthorAsync(Guid id);
    }
}