using System.Threading.Tasks;
using BookStore.Domain.Entities;

namespace BookStore.Data.Repositories.Contracts
{
    public interface IBookRepository : IRepository<Book>
    {
        public Task CreateBookAsync(Book book);
        public Task UpdateBookAsync(Book book);
    }
}