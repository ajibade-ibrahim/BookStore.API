using System.Threading.Tasks;
using BookStore.Domain.Entities;

namespace BookStore.Data.Repositories.Contracts
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task CreateAuthor(Author author);
        Task UpdateAuthor(Author author);
    }
}